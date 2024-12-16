using Sob_Need_Server.DTOs.Category;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Product;


namespace Sob_Need_Server.Interface
{
    public interface IProductService : IGenericService<ProductDto, int>
    {
        Task<ResponseDto<IEnumerable<ProductDto>>> GetAllProductsAsync();
        //Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        //Task<ProductDto> GetProductByIdAsync(int id);
        //Task<ProductDto> CreateProductAsync(ProductDto productDto);
        //Task<ProductDto> UpdateProductAsync(int id, ProductDto productDto);
        //Task<bool> DeleteProductAsync(int id);
    }
}
