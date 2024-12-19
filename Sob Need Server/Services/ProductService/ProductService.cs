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

namespace Sob_Need_Server.Services.ProductService
{
    public class ProductService(AppDbContext context, IMapper mapper) : IProductService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseDto<IEnumerable<ProductDto>>> GetAllProductsAsync(ProductFilterDto filters)
        {
            try
            {
                // Base query to fetch raw data
                var query = from product in _context.Products
                            join location in _context.Locations
                            on product.ProductId equals location.ProductId into locationGroup
                            from loc in locationGroup.DefaultIfEmpty()
                            join category in _context.Categories
                            on product.CategoryId equals category.CategoryId
                            select new
                            {
                                product,
                                loc,
                                category
                            };

                // Apply filters
                if (filters.CategoryId.HasValue)
                {
                    query = query.Where(x => x.product.CategoryId == filters.CategoryId.Value);
                }

                if (!string.IsNullOrWhiteSpace(filters.SearchText))
                {
                    query = query.Where(x =>
                        x.product.Title.Contains(filters.SearchText) ||
                        x.product.Description.Contains(filters.SearchText));
                }

                if (filters.OrderByAsc.HasValue)
                {
                    query = filters.OrderByAsc.Value
                        ? query.OrderBy(x => x.product.CreatedAt)
                        : query.OrderByDescending(x => x.product.CreatedAt);
                }

                // Apply pagination
                if (filters.Page.HasValue && filters.PageSize.HasValue)
                {
                    var skip = (filters.Page.Value - 1) * filters.PageSize.Value;
                    query = query.Skip(skip).Take(filters.PageSize.Value);
                }

                // Execute the query and fetch data
                var rawData = await query.ToListAsync();

                // Process attachments (client-side processing)
                var products = rawData.Select(x => new ProductDto
                {
                    ProductId = x.product.ProductId,
                    Title = x.product.Title,
                    Description = x.product.Description,
                    Price = x.product.Price,
                    CreatedAt = x.product.CreatedAt,
                    CategoryName = x.category.CategoryName,
                    Location = x.loc != null ? new LocationDto
                    {
                        AddressLine = x.loc.AddressLine,
                        CountryId = x.loc.CountryId
                    } : null!,
                    Images = _context.Attachments
                        .Where(a => a.ProductId == x.product.ProductId)
                        .Select(a => new ImageDto
                        {
                            ImageUrl = a.ImageUrl,
                            AltName = a.AltName
                        }).ToList()
                }).ToList();

                return new ResponseDto<IEnumerable<ProductDto>>(true, "Products retrieved successfully", products);
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
                var product = _mapper.Map<Product>(productDto);

                // Handle Location
                if (productDto.Location != null)
                {
                    var location = _mapper.Map<Location>(productDto.Location);
                    _context.Locations.Add(location);
                    await _context.SaveChangesAsync();
                }

                // Add Product
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                // Handle Images (Attachments)
                if (productDto.Images != null && productDto.Images.Count > 0)
                {
                    foreach (var imageDto in productDto.Images)
                    {
                        var attachment = new Attachment
                        {
                            ImageUrl = imageDto.ImageUrl,
                            AltName = imageDto.AltName,
                            CreatedAt = DateTime.UtcNow,
                            IsActive = true
                        };
                        _context.Attachments.Add(attachment);
                    }
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

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
                                        where p.ProductId == id
                                        select new ProductDto
                                        {
                                            ProductId = p.ProductId,
                                            Title = p.Title,
                                            Description = p.Description,
                                            Price = p.Price,
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
    }
}
