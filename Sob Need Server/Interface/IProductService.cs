using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Product;


namespace Sob_Need_Server.Interface
{
    public interface IProductService : IGenericService<ProductDto, int>
    {
        Task<ResponseDto<IEnumerable<ProductDto>>> GetAllProductsAsync(ProductFilterDto Filters);
        Task<ResponseDto<ProductDto>> GetProductBySlugAsync(string Slug);
    }
}
