using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.User;
using Sob_Need_Server.Interface.User;
using Sob_Need_Server.Utils;
using Sob_Need_Server.Utils.Helpers;

namespace Sob_Need_Server.Controllers.User
{
    [Route(Constants.BasePath + "/[controller]")]
    [ApiController]
    public class ReviewController(IReviewService reviewService, IHttpContextAccessor httpContextAccessor) : ControllerBase
    {
        private readonly IReviewService _reviewService = reviewService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        /// <summary>
        /// Add a review for a seller
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDto<ReviewDto>>> AddReview([FromBody] ReviewDto reviewDto)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));

            var response = await _reviewService.AddReviewAsync(userId.Value, reviewDto);
            
            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        /// <summary>
        /// Get all reviews for a seller
        /// </summary>
        [HttpGet("seller/{sellerId}")]
        public async Task<ActionResult<ResponseDto<IEnumerable<ReviewDto>>>> GetSellerReviews(Guid sellerId)
        {
            var response = await _reviewService.GetSellerReviewsAsync(sellerId);
            
            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        /// <summary>
        /// Get average rating for a seller
        /// </summary>
        [HttpGet("seller/{sellerId}/rating")]
        public async Task<ActionResult<ResponseDto<double>>> GetAverageRating(Guid sellerId)
        {
            var response = await _reviewService.GetAverageRatingAsync(sellerId);
            
            return Ok(response);
        }

        private Guid? GetCurrentUserId()
        {
            var userIdStr = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdStr)) return null;
            return Guid.Parse(userIdStr);
        }
    }
}
