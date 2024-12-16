using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Product;
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

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _context.Products.ToListAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;

            return _mapper.Map<ProductDto>(product);
        }
        public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
        {
            // Start a transaction
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Map ProductDto to Product entity
                var product = _mapper.Map<Product>(productDto);

                // Handle Location
                if (productDto.Location != null)
                {
                    var location = _mapper.Map<Location>(productDto.Location);
                    _context.Locations.Add(location);
                    await _context.SaveChangesAsync();
                }

                // Add Product to the database
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                // Handle Images (Attachments)
                if (productDto.Images != null && productDto.Images.Any())
                {
                    foreach (var imageDto in productDto.Images)
                    {
                        var attachment = new Attachment()
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

                // Commit the transaction
                await transaction.CommitAsync();

             return _mapper.Map<ProductDto>(product);


            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of any error
                await transaction.RollbackAsync();

                // Log the exception (optional)
                Console.WriteLine($"Error creating product: {ex.Message}");

                // Re-throw the exception to handle it further up the stack
                throw;
            }
        }


        public async Task<ProductDto> UpdateProductAsync(int id, ProductDto productDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;

            _mapper.Map(productDto, product);
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
