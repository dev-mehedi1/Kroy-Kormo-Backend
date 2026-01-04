using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.User;
using Sob_Need_Server.Interface.User;
using Sob_Need_Server.Utils;
using Sob_Need_Server.Utils.Helpers;

namespace Sob_Need_Server.Controllers.User
{
    [Route(Constants.BasePath + "/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoriteController(IFavoriteService favoriteService, IHttpContextAccessor httpContextAccessor) : ControllerBase
    {
        private readonly IFavoriteService _favoriteService = favoriteService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        /// <summary>
        /// Add product to favorites
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ResponseDto<FavoriteDto>>> AddFavorite([FromBody] Guid productId)
        {
            var userIdStr = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized(new ResponseDto<FavoriteDto>(false, "Unauthorized", null!));

            var response = await _favoriteService.AddFavoriteAsync(Guid.Parse(userIdStr), productId);
            
            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        /// <summary>
        /// Remove product from favorites
        /// </summary>
        [HttpDelete("{productId}")]
        public async Task<ActionResult<ResponseDto<bool>>> RemoveFavorite(Guid productId)
        {
            var userIdStr = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized(new ResponseDto<bool>(false, "Unauthorized", false));

            var response = await _favoriteService.RemoveFavoriteAsync(Guid.Parse(userIdStr), productId);
            
            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        /// <summary>
        /// Get user's favorite products
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<FavoriteDto>>>> GetFavorites()
        {
            var userIdStr = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized(new ResponseDto<IEnumerable<FavoriteDto>>(false, "Unauthorized", null!));

            var response = await _favoriteService.GetUserFavoritesAsync(Guid.Parse(userIdStr));
            
            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        /// <summary>
        /// Check if product is favorited
        /// </summary>
        [HttpGet("check/{productId}")]
        public async Task<ActionResult<ResponseDto<bool>>> IsFavorited(Guid productId)
        {
            var userIdStr = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized(new ResponseDto<bool>(false, "Unauthorized", false));

            var response = await _favoriteService.IsFavoritedAsync(Guid.Parse(userIdStr), productId);
            
            return Ok(response);
        }
    }
}
