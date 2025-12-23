using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.Models.Products;
using Sob_Need_Server.DTOs.Product;

namespace Sob_Need_Server.Interface.Products
{
    public interface IPromotionService
    {
        Task<ResponseDto<IEnumerable<PromotionDto>>> GetActivePromotionsAsync(Guid productId);
        Task<ResponseDto<PromotionDto>> ApplyPromotionAsync(Guid productId, string promotionType, int durationDays, Guid userId);
        Task<ResponseDto<bool>> DeactivatePromotionAsync(Guid promotionId, Guid userId);
    }
}
