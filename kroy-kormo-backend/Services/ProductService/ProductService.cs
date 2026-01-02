using AutoMapper;
using DTOs.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Models.Locations;
using Models.Products;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Attachment;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Location;
using Sob_Need_Server.DTOs.Product;
using Sob_Need_Server.Interface;
using Sob_Need_Server.Interface.Analytics;
using Sob_Need_Server.Models.Products;
using Sob_Need_Server.SignalR;
using Sob_Need_Server.Utils.Helpers;
using AttachmentEntity = Models.Attachment.Attachment;

namespace Sob_Need_Server.Services.ProductService;

public class ProductService(AppDbContext context, IMapper mapper, IHubContext<NotificationHub> hubContext, IHttpContextAccessor httpContextAccessor, IAnalyticsService analyticsService) : IProductService
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly IHubContext<NotificationHub> _hubContext = hubContext;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IAnalyticsService _analyticsService = analyticsService;



    public async Task<ResponseDto<IEnumerable<ProductDto>>> GetAllProductsAsync(ProductFilterDto filters)
    {
        try
        {
            // If CategorySlug is provided, fetch the category and its subcategories
            List<Guid> categoryIds = [];
            if (!string.IsNullOrWhiteSpace(filters.CategorySlug))
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Slug == filters.CategorySlug);
                if (category == null) return new ResponseDto<IEnumerable<ProductDto>>(false, "Category not found", null!);

                // Fetch subcategories of the specified category
                var subCategoryIds = await _context.Categories
                    .Where(c => c.ParentCategoryId == category.CategoryId)
                    .Select(c => c.CategoryId)
                    .ToListAsync();

                categoryIds.Add(category.CategoryId);
                categoryIds.AddRange(subCategoryIds);
            }

            // Base query to fetch products with navigation properties
            var query = _context.Products
                .Include(p => p.User)
                    .ThenInclude(u => u!.UserType)
                .Include(p => p.Category)
                .Include(p => p.Condition)
                .Include(p => p.Status)
                .Include(p => p.StatusHistories)
                    .ThenInclude(h => h.Status)
                .Include(p => p.Locations)
                    .ThenInclude(l => l.Country)
                .Include(p => p.Locations)
                    .ThenInclude(l => l.State)
                .Include(p => p.Locations)
                    .ThenInclude(l => l.City)
                .Include(p => p.Locations)
                    .ThenInclude(l => l.Region)
                .Include(p => p.Attachments)
                .Include(p => p.Promotions)
                .AsQueryable();

            // Apply filters
            if (categoryIds.Count != 0) query = query.Where(x => categoryIds.Contains(x.CategoryId));
            
            if (!string.IsNullOrWhiteSpace(filters.SearchText)) query = query.Where(x => x.Title.Contains(filters.SearchText) || x.Description.Contains(filters.SearchText));

            // Ads Type (IsSell)
            if (filters.IsSell.HasValue) query = query.Where(x => x.IsSell == filters.IsSell);

            // Is Urgent
            if (filters.IsUrgentAd.HasValue) query = query.Where(x => x.IsUrgentAd == filters.IsUrgentAd);

            if (filters.UserId.HasValue) query = query.Where(x => x.UserId == filters.UserId);

            if (filters.CountryId.HasValue) query = query.Where(x => x.Locations.Any(l => l.CountryId == filters.CountryId));
            if (filters.StateId.HasValue) query = query.Where(x => x.Locations.Any(l => l.StateId == filters.StateId));
            if (filters.CityId.HasValue) query = query.Where(x => x.Locations.Any(l => l.CityId == filters.CityId));
            if (filters.RegionId.HasValue) query = query.Where(x => x.Locations.Any(l => l.RegionId == filters.RegionId));

            // Is top ads
            if (filters.IsTopAd.HasValue)
            {
                query = query.Where(x => x.IsTopAd == filters.IsTopAd);
            }

            // Default visibility logic
            if (filters.IsManagementMode == true)
            {
                // In management mode, show everything for the user
                if (filters.UserId.HasValue)
                {
                    query = query.Where(x => x.UserId == filters.UserId);
                }
            }
            else
            {
                // In public mode, filter out sold and Canceled
                query = query.Where(x => x.Status != null && 
                                        x.Status.Slug != "sold" && 
                                        x.Status.Slug != "canceled");
                
                // If UserId is provided in public mode (e.g. seller store), still exclude sold/Canceled
                if (filters.UserId.HasValue)
                {
                    query = query.Where(x => x.UserId == filters.UserId);
                }
            }

            if (filters.OrderByAsc.HasValue)
            {
                query = filters.OrderByAsc.Value
                    ? query.OrderBy(x => x.CreatedAt)
                    : query.OrderByDescending(x => x.CreatedAt);
            }
            // Total count before paging
            var totalCount = await query.CountAsync();

            if (filters.Page.HasValue && filters.PageSize.HasValue)
            {
                var skip = (filters.Page.Value - 1) * filters.PageSize.Value;
                query = query.Skip(skip).Take(filters.PageSize.Value);
            }

            // Execute query and process the data
            var rawData = await query.ToListAsync();

            var products = _mapper.Map<IEnumerable<ProductDto>>(rawData);

            return new ResponseDto<IEnumerable<ProductDto>>(true, "Products retrieved successfully", products)
            {
                TotalCount = totalCount // total count add here
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<IEnumerable<ProductDto>>(false, ex.Message, null!);
        }
    }

    public async Task<ResponseDto<ProductDto>> CreateAsync(CreateProductDto productDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Check if slug already exists in Products table
            var existSlug = await _context.Products.AnyAsync(x => x.Title == productDto.Title);
            if (existSlug) return new ResponseDto<ProductDto>(false, "A product with the same title already exists.", null!);

            // Get UserId from token
            var authHeader = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();
            var userIdStr = JwtHelper.GetUserIdFromToken(authHeader!);
            if (string.IsNullOrEmpty(userIdStr)) return new ResponseDto<ProductDto>(false, "Unauthorized", null!);
            
            var userId = Guid.Parse(userIdStr);

            var product = _mapper.Map<Product>(productDto);
            
            // Set fields from token/system
            product.UserId = userId;
            product.Slug = StringUtilities.GenerateSlug(product.Title);
            product.CreatedAt = DateTime.UtcNow;
            product.CreatedBy = userIdStr;
            product.IsVerified = false;

            // Set default status (e.g., Pending)
            var pendingStatus = await _context.ProductStatuses.FirstOrDefaultAsync(s => s.Slug == "pending");
            if (pendingStatus != null)
            {
                product.StatusId = pendingStatus.StatusId;
            }

            // Add Product
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Record initial status history
            if (pendingStatus != null)
            {
                var history = new ProductStatusHistory
                {
                    ProductId = product.ProductId,
                    StatusId = pendingStatus.StatusId,
                    ChangedBy = userIdStr,
                    Notes = "Initial status on creation",
                    CreatedAt = DateTime.UtcNow
                };
                _context.ProductStatusHistories.Add(history);
                await _context.SaveChangesAsync();
            }

            // Handle Images (Attachments)
            if (productDto.ImageUrls != null && productDto.ImageUrls.Count > 0)
            {
                foreach (var imageUrl in productDto.ImageUrls)
                {
                    var attachment = new AttachmentEntity
                    {
                        ImageUrl = imageUrl,
                        ProductId = product.ProductId,
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true
                    };
                    _context.Attachments.Add(attachment);
                }
            }

            // Handle Attributes
            if (productDto.Attributes != null && productDto.Attributes.Count > 0)
            {
                foreach (var attrDto in productDto.Attributes)
                {
                    var attrValue = _mapper.Map<ProductAttributeValue>(attrDto);
                    attrValue.ProductId = product.ProductId;
                    _context.ProductAttributeValues.Add(attrValue);
                }
            }

            // Handle Multiple Locations
            if (productDto.Locations != null && productDto.Locations.Count > 0)
            {
                foreach (var locDto in productDto.Locations)
                {
                    var location = _mapper.Map<Location>(locDto);
                    location.ProductId = product.ProductId;
                    location.CreatedAt = DateTime.UtcNow;
                    location.IsActive = true;
                    // Ensure ID is generated if not provided
                    if (location.LocationId == Guid.Empty) location.LocationId = Guid.NewGuid();
                    
                    _context.Locations.Add(location);
                }
            }


            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            // Notify via SignalR
            string notificationMessage = $"New product '{product.Title}' listed.";
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", "SYSTEM", notificationMessage, "GLOBAL");

            var createdProductDto = _mapper.Map<ProductDto>(product);
            return new ResponseDto<ProductDto>(true, "Product created successfully", createdProductDto);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new ResponseDto<ProductDto>(false, ex.Message, null!);
        }
    }

    public async Task<ResponseDto<ProductDto>> UpdateAsync(Guid id, UpdateProductDto productDto)
    {
        try
        {
            var product = await _context.Products
                .Include(p => p.Attachments)
                .Include(p => p.AttributeValues)
                .Include(p => p.Locations)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
                return new ResponseDto<ProductDto>(false, "Product not found", null!);

            // Map updates
            _mapper.Map(productDto, product);
            product.UpdatedAt = DateTime.UtcNow;

            // Handle Locations Update (Replace All strategy)
            if (productDto.Locations != null)
            {
                // Remove existing locations
                _context.Locations.RemoveRange(product.Locations);
                
                // Add new locations
                foreach (var locDto in productDto.Locations)
                {
                    var location = _mapper.Map<Location>(locDto);
                    location.ProductId = product.ProductId;
                    location.CreatedAt = DateTime.UtcNow;
                    location.IsActive = true;
                     if (location.LocationId == Guid.Empty) location.LocationId = Guid.NewGuid();
                    _context.Locations.Add(location);
                }
            }

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            var updatedProductDto = _mapper.Map<ProductDto>(product);
            return new ResponseDto<ProductDto>(true, "Product updated successfully", updatedProductDto);
        }
        catch (Exception ex)
        {
            return new ResponseDto<ProductDto>(false, ex.Message, null!);
        }
    }

    public async Task<ResponseDto<ProductDto>> DeleteAsync(Guid id, Guid userId)
    {
        try
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return new ResponseDto<ProductDto>(false, "Product not found", null!);

            // Security check
            if (product.UserId != userId)
            {
                return new ResponseDto<ProductDto>(false, "You do not have permission to delete this product", null!);
            }

            var productDto = _mapper.Map<ProductDto>(product);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return new ResponseDto<ProductDto>(true, "Product deleted successfully", productDto);
        }
        catch (Exception ex)
        {
            return new ResponseDto<ProductDto>(false, ex.Message, null!);
        }
    }

    public async Task<ResponseDto<ProductDto>> GetByIdAsync(Guid id)
    {
        try
        {
            var product = await _context.Products
                .Include(p => p.User)
                    .ThenInclude(u => u!.UserType)
                .Include(p => p.Category)
                .Include(p => p.Condition)
                .Include(p => p.Status)
                .Include(p => p.Locations)
                    .ThenInclude(l => l.Country)
                .Include(p => p.Locations)
                    .ThenInclude(l => l.State)
                .Include(p => p.Locations)
                    .ThenInclude(l => l.City)
                .Include(p => p.Locations)
                    .ThenInclude(l => l.Region)
                .Include(p => p.Attachments)
                .Include(p => p.Promotions)
                .Include(p => p.StatusHistories)
                    .ThenInclude(h => h.Status)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
                return new ResponseDto<ProductDto>(false, "Product not found", null!);

            var productDto = _mapper.Map<ProductDto>(product);

            if (productDto == null)
                return new ResponseDto<ProductDto>(false, "Product not found", null!);

            // Track View Telemetry
            var authHeader = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();
            var userIdStr = JwtHelper.GetUserIdFromToken(authHeader!);
            Guid? userId = string.IsNullOrEmpty(userIdStr) ? null : Guid.Parse(userIdStr);
            var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var userAgent = _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString() ?? "Unknown";

            // Fire and forget view tracking to avoid blocking response
            _ = _analyticsService.TrackProductViewAsync(id, userId, ipAddress, userAgent);

            return new ResponseDto<ProductDto>(true, "Product retrieved successfully", productDto);
        }
        catch (Exception ex)
        {
            return new ResponseDto<ProductDto>(false, ex.Message, null!);
        }
    }


    public async Task<ResponseDto<ProductDto>> GetProductBySlugAsync(string Slug)
    {
        try
        {
            var product = await _context.Products
                    .Include(p => p.User)
                        .ThenInclude(u => u!.UserType)
                    .Include(p => p.Category)
                    .Include(p => p.Condition)
                    .Include(p => p.Status)
                    .Include(p => p.Locations)         
                        .ThenInclude(l => l.Country)
                    .Include(p => p.Locations)
                        .ThenInclude(l => l.State)
                    .Include(p => p.Locations)
                        .ThenInclude(l => l.City)
                    .Include(p => p.Locations)
                        .ThenInclude(l => l.Region)
                    .Include(p => p.Attachments)
                    .Include(p => p.Promotions)
                    .Include(p => p.StatusHistories)
                        .ThenInclude(h => h.Status)
                    .FirstOrDefaultAsync(p => p.Slug == Slug);

            if (product == null) return new ResponseDto<ProductDto>(false, "Product not found", null!);

            var productDto = _mapper.Map<ProductDto>(product);

            if (productDto == null) return new ResponseDto<ProductDto>(false, "Product not found", null!);

            // Track View Telemetry
            var authHeader = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();
            var userIdStr = JwtHelper.GetUserIdFromToken(authHeader!);
            Guid? userId = string.IsNullOrEmpty(userIdStr) ? null : Guid.Parse(userIdStr);
            var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var userAgent = _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString() ?? "Unknown";

            // Fire and forget view tracking
            _ = _analyticsService.TrackProductViewAsync(productDto.ProductId, userId, ipAddress, userAgent);

            return new ResponseDto<ProductDto>(true, "Product retrieved successfully", productDto);
        }
        catch(Exception ex)
        {
            return new ResponseDto<ProductDto>(false, $"{ex.Message}", null!);
        }
    }

    public async Task<ResponseDto<ProductDto>> UpdateStatusAsync(Guid id, string statusSlug, Guid userId)
    {
        try
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return new ResponseDto<ProductDto>(false, "Product not found", null!);

            // Security check
            if (product.UserId != userId)
            {
                return new ResponseDto<ProductDto>(false, "You do not have permission to update this product status", null!);
            }

            var status = await _context.ProductStatuses.FirstOrDefaultAsync(s => s.Slug == statusSlug);
            if (status == null) return new ResponseDto<ProductDto>(false, "Status not found", null!);
            product.StatusId = status.StatusId;
            product.UpdatedAt = DateTime.UtcNow;

            _context.Products.Update(product);

            // Record status history
            var history = new ProductStatusHistory
            {
                ProductId = id,
                StatusId = status.StatusId,
                ChangedBy = userId.ToString(),
                CreatedAt = DateTime.UtcNow
            };
            _context.ProductStatusHistories.Add(history);

            await _context.SaveChangesAsync();

            var updatedProductDto = _mapper.Map<ProductDto>(product);
            return new ResponseDto<ProductDto>(true, "Product status updated successfully", updatedProductDto);
        }
        catch (Exception ex)
        {
            return new ResponseDto<ProductDto>(false, ex.Message, null!);
        }
    }

    public async Task<ResponseDto<ProductDto>> UpdateStatusByIdAsync(Guid id, Guid statusId, Guid userId)
    {
        try
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return new ResponseDto<ProductDto>(false, "Product not found", null!);

            // Security check
            if (product.UserId != userId)
            {
                return new ResponseDto<ProductDto>(false, "You do not have permission to update this product status", null!);
            }

            var status = await _context.ProductStatuses.FindAsync(statusId);
            if (status == null) return new ResponseDto<ProductDto>(false, "Status not found", null!);

            product.StatusId = statusId;
            product.UpdatedAt = DateTime.UtcNow;

            _context.Products.Update(product);

            // Record status history
            var history = new ProductStatusHistory
            {
                ProductId = id,
                StatusId = statusId,
                ChangedBy = userId.ToString(),
                CreatedAt = DateTime.UtcNow
            };
            _context.ProductStatusHistories.Add(history);

            await _context.SaveChangesAsync();

            var updatedProductDto = _mapper.Map<ProductDto>(product);
            return new ResponseDto<ProductDto>(true, "Product status updated successfully", updatedProductDto);
        }
        catch (Exception ex)
        {
            return new ResponseDto<ProductDto>(false, ex.Message, null!);
        }
    }

    public async Task<ResponseDto<IEnumerable<ProductStatusHistoryDto>>> GetStatusHistoryAsync(Guid productId)
    {
        try
        {
            var history = await _context.ProductStatusHistories
                .Include(h => h.Status)
                .Where(h => h.ProductId == productId)
                .OrderBy(h => h.CreatedAt)
                .ToListAsync();

            var historyDto = _mapper.Map<IEnumerable<ProductStatusHistoryDto>>(history);
            return new ResponseDto<IEnumerable<ProductStatusHistoryDto>>(true, "Status history retrieved successfully", historyDto);
        }
        catch (Exception ex)
        {
            return new ResponseDto<IEnumerable<ProductStatusHistoryDto>>(false, ex.Message, null!);
        }
    }

    public async Task<ResponseDto<IEnumerable<ProductDto>>> GetMostViewedProductsAsync(int count)
    {
        try
        {
            var products = await _context.Products
                .Include(p => p.User)
                .Include(p => p.Category)
                .Include(p => p.Condition)
                .Include(p => p.Status)
                .Include(p => p.Attachments)
                .Include(p => p.Locations)
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.ViewCount)
                .Take(count)
                .ToListAsync();

            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);
            return new ResponseDto<IEnumerable<ProductDto>>(true, "Most viewed products retrieved successfully", productsDto);
        }
        catch (Exception ex)
        {
            return new ResponseDto<IEnumerable<ProductDto>>(false, ex.Message, null!);
        }
    }

    public async Task<ResponseDto<IEnumerable<ProductDto>>> GetRelatedProductsAsync(Guid productId, int count)
    {
        try
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return new ResponseDto<IEnumerable<ProductDto>>(false, "Product not found", null!);

            var relatedProducts = await _context.Products
                .Include(p => p.User)
                .Include(p => p.Category)
                .Include(p => p.Condition)
                .Include(p => p.Status)
                .Include(p => p.Attachments)
                .Include(p => p.Locations)
                .Where(p => p.CategoryId == product.CategoryId && p.ProductId != productId && p.IsActive)
                .OrderByDescending(p => p.IsTopAd)
                .ThenByDescending(p => p.IsFeaturedMember)
                .ThenByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync();

            var relatedProductsDto = _mapper.Map<IEnumerable<ProductDto>>(relatedProducts);
            return new ResponseDto<IEnumerable<ProductDto>>(true, "Related products retrieved successfully", relatedProductsDto);
        }
        catch (Exception ex)
        {
            return new ResponseDto<IEnumerable<ProductDto>>(false, ex.Message, null!);
        }
    }

    public async Task<ResponseDto<ProductDto>> CancelAsync(Guid id, string reason, Guid userId)
    {
        try
        {
            var product = await _context.Products
                .Include(p => p.Status)
                .FirstOrDefaultAsync(p => p.ProductId == id);
            
            if (product == null)
                return new ResponseDto<ProductDto>(false, "Product not found", null!);

            if (product.UserId != userId)
                return new ResponseDto<ProductDto>(false, "Unauthorized", null!);

            // Find "Canceled" status
            var canceledStatus = await _context.ProductStatuses
                .FirstOrDefaultAsync(s => s.StatusName == "Canceled");
            
            if (canceledStatus == null)
            {
                // Fallback: Create it if it doesn't exist, though it should be seeded
                canceledStatus = new ProductStatus { StatusId = Guid.NewGuid(), StatusName = "Canceled", Slug = "canceled" };
                _context.ProductStatuses.Add(canceledStatus);
                await _context.SaveChangesAsync();
            }

            product.StatusId = canceledStatus.StatusId;
            product.UpdatedAt = DateTime.UtcNow;

            // Add history
            var history = new ProductStatusHistory
            {
                HistoryId = Guid.NewGuid(),
                ProductId = product.ProductId,
                StatusId = canceledStatus.StatusId,
                Notes = reason,
                ChangedBy = userId.ToString(),
                CreatedAt = DateTime.UtcNow
            };
            _context.ProductStatusHistories.Add(history);

            await _context.SaveChangesAsync();
            
            var productDto = _mapper.Map<ProductDto>(product);
            return new ResponseDto<ProductDto>(true, "Product canceled successfully", productDto);
        }
        catch (Exception ex)
        {
            return new ResponseDto<ProductDto>(false, ex.Message, null!);
        }
    }
}
