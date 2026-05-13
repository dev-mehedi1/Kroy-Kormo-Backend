using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Product;

namespace Sob_Need_Server.Interface.Products
{
    public interface IProductAttributeService
    {
        Task<ResponseDto<IEnumerable<ProductAttributeDto>>> GetAttributesByCategoryAsync(Guid categoryId);
        Task<ResponseDto<IEnumerable<ProductAttributeDto>>> GetAttributesByCategorySlugAsync(string slug);
        Task<ResponseDto<ProductAttributeDto>> GetAttributeByIdAsync(Guid attributeId);
        Task<ResponseDto<IEnumerable<ProductAttributeDto>>> GetAllAttributesAsync();
        Task<ResponseDto<ProductAttributeDto>> CreateAttributeAsync(ProductAttributeDto attributeDto);
        Task<ResponseDto<ProductAttributeDto>> UpdateAttributeAsync(Guid attributeId, ProductAttributeDto attributeDto);
        Task<ResponseDto<bool>> DeleteAttributeAsync(Guid attributeId);
    }
}
