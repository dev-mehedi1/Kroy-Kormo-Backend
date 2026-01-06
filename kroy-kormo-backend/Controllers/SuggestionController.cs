using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.Common;
using DTOs.Product;
using Sob_Need_Server.Interface;
using Sob_Need_Server.Utils.Helpers;

namespace Sob_Need_Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuggestionController(ISuggestionService suggestionService, IHttpContextAccessor httpContextAccessor) : ControllerBase
    {
        private readonly ISuggestionService _suggestionService = suggestionService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        private Guid? GetUserId()
        {
             var authHeader = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();
             if (string.IsNullOrEmpty(authHeader)) return null;
             
             var userIdStr = JwtHelper.GetUserIdFromToken(authHeader);
             if (Guid.TryParse(userIdStr, out Guid userId)) return userId;
             return null;
        }

        [HttpGet("recommended")]
        public async Task<ResponseDto<IEnumerable<ProductDto>>> GetRecommended([FromQuery] int count = 10)
        {
            var userId = GetUserId();
            if (!userId.HasValue) return new ResponseDto<IEnumerable<ProductDto>>(false, "User not authenticated", null!);
            
            return await _suggestionService.GetRecommendedAdsAsync(userId.Value, count);
        }

        [HttpGet("nearby")]
        public async Task<ResponseDto<IEnumerable<ProductDto>>> GetNearby([FromQuery] double? lat, [FromQuery] double? lon, [FromQuery] int count = 10)
        {
            var userId = GetUserId();
            if (!userId.HasValue) return new ResponseDto<IEnumerable<ProductDto>>(false, "User not authenticated", null!);

            return await _suggestionService.GetNearYouAsync(userId.Value, lat, lon, count);
        }

        [HttpGet("trending")]
        public async Task<ResponseDto<IEnumerable<ProductDto>>> GetTrending([FromQuery] int count = 10)
        {
            var userId = GetUserId();
            // Allow trending (maybe) for guests? But logic requires exclude self. Guest logic can be added later.
            // For now assuming Auth.
            if (!userId.HasValue) return new ResponseDto<IEnumerable<ProductDto>>(false, "User not authenticated", null!);

            return await _suggestionService.GetTrendingAsync(userId.Value, count);
        }

        [HttpGet("similar-history")]
        public async Task<ResponseDto<IEnumerable<ProductDto>>> GetSimilarHistory([FromQuery] int count = 10)
        {
            var userId = GetUserId();
            if (!userId.HasValue) return new ResponseDto<IEnumerable<ProductDto>>(false, "User not authenticated", null!);

            return await _suggestionService.GetRecentlyViewedSimilarAsync(userId.Value, count);
        }
    }
}
