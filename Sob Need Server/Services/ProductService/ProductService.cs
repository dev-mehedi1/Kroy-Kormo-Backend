using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Product;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.Interface;
using Sob_Need_Server.Models.Attachment;
using Sob_Need_Server.Models.Locations;
using Sob_Need_Server.Models.Products;
using Sob_Need_Server.DTOs.Location;
using Sob_Need_Server.DTOs.Attachment;
using Sob_Need_Server.Utils.Helpers;
using Microsoft.AspNetCore.SignalR;
using Sob_Need_Server.SignalR;
using Microsoft.AspNetCore.Http;

namespace Sob_Need_Server.Services.ProductService;

public class ProductService(AppDbContext context, IMapper mapper, IHubContext<NotificationHub> hubContext, IHttpContextAccessor httpContextAccessor) : IProductService
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly IHubContext<NotificationHub> _hubContext = hubContext;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;



    public async Task<ResponseDto<IEnumerable<ProductDto>>> GetAllProductsAsync(ProductFilterDto filters)
    {
        try
        {
            // If CategorySlug is provided, fetch the category and its subcategories
            List<int> categoryIds = new();
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

            // Base query to fetch products
            var query = from product in _context.Products
                        join location in _context.Locations on product.ProductId equals location.ProductId into locationGroup
                        from loc in locationGroup.DefaultIfEmpty()

                        join category in _context.Categories on product.CategoryId equals category.CategoryId into categoryG
                        from category in categoryG.DefaultIfEmpty()

                        join c in _context.Countries on loc != null ? loc.CountryId : (int?)null equals c.CountryId into cGroup
                        from c in cGroup.DefaultIfEmpty()

                        join s in _context.States on loc != null ? loc.StateId : (int?)null equals s.StateId into sGroup
                        from s in sGroup.DefaultIfEmpty()

                        join ci in _context.Cities on loc != null ? loc.CityId : (int?)null equals ci.CityId into ciGroup
                        from ci in ciGroup.DefaultIfEmpty()

                        join r in _context.Regions on loc != null ? loc.RegionId : (int?)null equals r.RegionId into rGroup
                        from r in rGroup.DefaultIfEmpty()

                        select new
                        {
                            Product = product,
                            Location = loc,
                            Category = category,
                            CountryId = c != null ? c.CountryId : (int?)null,
                            CountryName = c != null ? c.CountryName : null,
                            StateId = s != null ? s.StateId : (int?)null,
                            StateName = s != null ? s.StateName : null,
                            CityId = ci != null ? ci.CityId : (int?)null,
                            CityName = ci != null ? ci.CityName : null,
                            RegionId = r != null ? r.RegionId : (int?)null,
                            RegionName = r != null ? r.RegionName : null
                        };

            // Apply filters
            if (categoryIds.Count != 0) query = query.Where(x => categoryIds.Contains(x.Product.CategoryId));
            
            if (!string.IsNullOrWhiteSpace(filters.SearchText)) query = query.Where(x => x.Product.Title.Contains(filters.SearchText) || x.Product.Description.Contains(filters.SearchText));

            // Is Argent
            if (filters.IsArgent.HasValue) query = query.Where(x => x.Product.IsUrgentAd == filters.IsArgent);

            if (!string.IsNullOrWhiteSpace(filters.UserId)) query = query.Where(x => x.Product.CreatedBy == filters.UserId);

            if (filters.CountryId.HasValue) query = query.Where(x => x.CountryId == filters.CountryId);
            if (filters.StateId.HasValue) query = query.Where(x => x.StateId == filters.StateId);
            if (filters.CityId.HasValue) query = query.Where(x => x.CityId == filters.CityId);
            if (filters.RegionId.HasValue) query = query.Where(x => x.RegionId == filters.RegionId);

            // Is top ads
            if (filters.IsTop.HasValue)
            {
                query = query.Where(x => x.Product.IsTopAd == filters.IsTop);
            }

            if (filters.OrderByAsc.HasValue)
            {
                query = filters.OrderByAsc.Value
                    ? query.OrderBy(x => x.Product.CreatedAt)
                    : query.OrderByDescending(x => x.Product.CreatedAt);
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

            var products = rawData.Select(x => new ProductDto
            {
                ProductId = x.Product.ProductId,
                Title = x.Product.Title,
                Description = x.Product.Description,
                Price = x.Product.Price,
                CreatedAt = x.Product.CreatedAt,
                Slug = x.Product.Slug,
                IsSell = x.Product.IsSell,
                CategoryName = x?.Category?.CategoryName,
                Location = x?.Location != null ? new LocationDto
                {
                    AddressLine = x.Location.AddressLine ?? null,
                    CountryName = x.CountryName ?? null,
                    CityName = x.CityName ?? null,
                    StateName = x.StateName ?? null,
                    RegionName = x.RegionName ?? null,
                } : null!,
                Images = _context.Attachments
                    .Where(a => a.ProductId == x.Product.ProductId)
                    .Select(a => new ImageDto
                    {
                        ImageUrl = a.ImageUrl,
                        AltName = a.AltName
                    }).ToList()
            }).ToList();

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

    public async Task<ResponseDto<ProductDto>> CreateAsync(ProductDto productDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            //Check is already exist slug
            var existSlug = await _context.Categories.AnyAsync(x => x.Slug == productDto.Slug);
            if (existSlug) return new ResponseDto<ProductDto>(false, "A product with the same slug already exists.", null!);
            // Get UserId from token
            var authHeader = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();
            var userId = JwtHelper.GetUserIdFromToken(authHeader!);
            var product = _mapper.Map<Product>(productDto);
            var slug = StringUtilities.GenerateSlug(product.Slug);
            product.Slug = slug;
            product.IsSell = false;
            product.CreatedAt = DateTime.UtcNow;
            product.CreatedBy = userId;

            // Add Product
            _context.Products.Add(product);
     

            // Handle Location
            if (productDto.Location != null)
            {
                var location = _mapper.Map<Location>(productDto.Location);
                location.ProductId = product.ProductId;
                _context.Locations.Add(location);
            }


            // Handle Images (Attachments)
            if (productDto.Images != null && productDto.Images.Count > 0)
            {
                foreach (var imageDto in productDto.Images)
                {
                    var attachment = new Attachment
                    {
                        ImageUrl = imageDto.ImageUrl,
                        AltName = imageDto.AltName,
                        ProductId = product.ProductId,
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true
                    };
                    _context.Attachments.Add(attachment);
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            // send notification 
            // Create a notification message
            string notificationMessage = $"Product '{product.Title}' was created at {DateTime.UtcNow}.";
            await _hubContext.Clients.All.SendAsync(notificationMessage);

            var createdProductDto = _mapper.Map<ProductDto>(product);
            return new ResponseDto<ProductDto>(true, "Product created successfully", createdProductDto);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new ResponseDto<ProductDto>(false, ex.Message, null, 500);
        }
    }

    public async Task<ResponseDto<ProductDto>> UpdateAsync(int id, ProductDto productDto)
    {
        try
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return new ResponseDto<ProductDto>(false, "Product not found", null!);

            _mapper.Map(productDto, product);
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

    public Task<ResponseDto<bool>> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseDto<ProductDto>> GetByIdAsync(int id)
    {
        try
        {
            var productDto = await (from p in _context.Products
                                    join location in _context.Locations
                                    on p.ProductId equals location.ProductId into locationGroup
                                    from loc in locationGroup.DefaultIfEmpty()
                                    join attachment in _context.Attachments
                                    on p.ProductId equals attachment.ProductId into attachmentGroup
                                    join category in _context.Categories on p.CategoryId equals category.CategoryId
                                    where p.ProductId == id
                                    select new ProductDto
                                    {
                                        ProductId = p.ProductId,
                                        Title = p.Title,
                                        Description = p.Description,
                                        Price = p.Price,
                                        IsDeliveryFree = p.IsDeliveryFree,
                                        CategoryName = category.CategoryName,
                                        IsTopAd = p.IsTopAd,
                                        IsJobAd = p.IsJobAd,
                                        CreatedAt = p.CreatedAt,
                                        Location = new LocationDto 
                                        {
                                            AddressLine = loc.AddressLine,
                                            CountryId = loc.CountryId
                                        },
                                        Images = attachmentGroup.Select(a => new ImageDto
                                        {
                                            ImageUrl = a.ImageUrl,
                                            AltName = a.AltName
                                        }).ToList()
                                    }).FirstOrDefaultAsync();

            if (productDto == null)
                return new ResponseDto<ProductDto>(false, "Product not found", null!);

            return new ResponseDto<ProductDto>(true, "Product retrieved successfully", productDto);
        }
        catch (Exception ex)
        {
            return new ResponseDto<ProductDto>(false, ex.Message, null!);
        }
    }


    Task<ResponseDto<ProductDto>> IGenericService<ProductDto, int>.DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseDto<ProductDto>> GetProductBySlugAsync(string Slug)
    {
        try
        {
            var productDto = await (from p in _context.Products
                                    join location in _context.Locations
                                    on p.ProductId equals location.ProductId into locationGroup
                                    from loc in locationGroup.DefaultIfEmpty()
                                    join attachment in _context.Attachments
                                    on p.ProductId equals attachment.ProductId into attachmentGroup
                                    where p.Slug == Slug
                                    select new ProductDto
                                    {
                                        ProductId = p.ProductId,
                                        Title = p.Title,
                                        Description = p.Description,
                                        Price = p.Price,
                                        CreatedAt = p.CreatedAt,
                                        Slug= p.Slug,
                                        Location = new LocationDto
                                        {
                                            AddressLine = loc.AddressLine,
                                            CountryId = loc.CountryId
                                        },
                                        Images = attachmentGroup.Select(a => new ImageDto
                                        {
                                            ImageUrl = a.ImageUrl,
                                            AltName = a.AltName
                                        }).ToList()
                                    }).FirstOrDefaultAsync();

            if (productDto == null) return new ResponseDto<ProductDto>(false, "Product not found", null!);

            return new ResponseDto<ProductDto>(true, "Product retrieved successfully", productDto);
        }
        catch(Exception ex)
        {
            return new ResponseDto<ProductDto>(false, $"{ex.Message}", null!);
        }
    }
}
