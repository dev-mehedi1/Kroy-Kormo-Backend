using DTOs.Product;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Product;


namespace Sob_Need_Server.Interface
{
    public interface IProductService
    {
        Task<ResponseDto<ProductDto>> GetByIdAsync(Guid id);
        Task<ResponseDto<ProductDto>> CreateAsync(CreateProductDto productDto);
        Task<ResponseDto<ProductDto>> UpdateAsync(Guid id, UpdateProductDto productDto);
        Task<ResponseDto<ProductDto>> DeleteAsync(Guid id, Guid userId);
        Task<ResponseDto<IEnumerable<ProductDto>>> GetAllProductsAsync(ProductFilterDto Filters);
        Task<ResponseDto<ProductDto>> GetProductBySlugAsync(string Slug);
        Task<ResponseDto<ProductDto>> UpdateStatusAsync(Guid id, string statusSlug, Guid userId);
        Task<ResponseDto<ProductDto>> UpdateStatusByIdAsync(Guid id, Guid statusId, Guid userId);
        Task<ResponseDto<IEnumerable<ProductStatusHistoryDto>>> GetStatusHistoryAsync(Guid productId);
        Task<ResponseDto<IEnumerable<ProductDto>>> GetMostViewedProductsAsync(int count);
        Task<ResponseDto<IEnumerable<ProductDto>>> GetRelatedProductsAsync(Guid productId, int count);
    }
}
