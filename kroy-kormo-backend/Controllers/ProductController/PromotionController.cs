using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.Interface;
using Sob_Need_Server.Interface.Products;
using Sob_Need_Server.DTOs.Product;
using Sob_Need_Server.Utils;
using Sob_Need_Server.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sob_Need_Server.Controllers.ProductController
{
    [Route(Constants.BasePath + "/[controller]")]
    [ApiController]
    [Authorize]
    public class PromotionController(IPromotionService promotionService, IProductService productService, IHttpContextAccessor httpContextAccessor) : ControllerBase
    {
        private readonly IPromotionService _promotionService = promotionService;
        private readonly IProductService _productService = productService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        [HttpGet("active/{productId}")]
        public async Task<ActionResult<ResponseDto<IEnumerable<PromotionDto>>>> GetActivePromotions(Guid productId)
        {
            var res = await _promotionService.GetActivePromotionsAsync(productId);
            return Ok(res);
        }

        [HttpPost("apply")]
        public async Task<ActionResult<ResponseDto<PromotionDto>>> ApplyPromotion(Guid productId, string promotionType, int durationDays)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();
            
            var res = await _promotionService.ApplyPromotionAsync(productId, promotionType, durationDays, userId.Value);
            return Ok(res);
        }

        [HttpPost("deactivate/{promotionId}")]
        public async Task<ActionResult<ResponseDto<bool>>> DeactivatePromotion(Guid promotionId)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var res = await _promotionService.DeactivatePromotionAsync(promotionId, userId.Value);
            return Ok(res);
        }

        private Guid? GetCurrentUserId()
        {
            var authHeader = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();
            var userIdStr = JwtHelper.GetUserIdFromToken(authHeader!);
            if (string.IsNullOrEmpty(userIdStr)) return null;
            return Guid.Parse(userIdStr);
        }
    }
}
