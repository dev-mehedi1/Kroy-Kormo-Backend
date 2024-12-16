using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Product;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.Interface;
using Sob_Need_Server.Models.Attachment;
using Sob_Need_Server.Models.Locations;
using Sob_Need_Server.Models.Products;

namespace Sob_Need_Server.Services.ProductService
{
    public class ProductService(AppDbContext context, IMapper mapper) : IProductService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseDto<IEnumerable<ProductDto>>> GetAllProductsAsync()
        {
            try
            {
                var products = await _context.Products.ToListAsync();
                var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
                return new ResponseDto<IEnumerable<ProductDto>>(true, "Products retrieved successfully", productDtos);
            }
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<ProductDto>>(false, ex.Message, null!);
            }
        }

        public async Task<ResponseDto<ProductDto>> GetByIdAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return new ResponseDto<ProductDto>(false, "Product not found", null!);

                var productDto = _mapper.Map<ProductDto>(product);
                return new ResponseDto<ProductDto>(true, "Product retrieved successfully", productDto);
            }
            catch (Exception ex)
            {
                return new ResponseDto<ProductDto>(false, ex.Message, null!);
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
    }
}
