using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Jobs;
using Sob_Need_Server.Services.Jobs;
using System.Security.Claims;

namespace Sob_Need_Server.Controllers.Jobs
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class JobRecommendationController : ControllerBase
    {
        private readonly IJobRecommendationService _recommendationService;

        public JobRecommendationController(IJobRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        [Authorize]
        [HttpGet("for-you")]
        public async Task<IActionResult> GetPersonalizedRecommendations([FromQuery] int count = 20)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);

            var result = await _recommendationService.GetRecommendedJobsAsync(userId, count);
            return Ok(new ResponseDto<IEnumerable<JobWithScoreDto>>(true, "Personalized recommendations retrieved", result));
        }

        [HttpGet("trending")]
        public async Task<IActionResult> GetTrendingJobs([FromQuery] int count = 20)
        {
            var result = await _recommendationService.GetTrendingJobsAsync(count);
            return Ok(new ResponseDto<IEnumerable<TrendingJobDto>>(true, "Trending jobs retrieved", result));
        }

        [HttpGet("similar/{jobId}")]
        public async Task<IActionResult> GetSimilarJobs(Guid jobId, [FromQuery] int count = 10)
        {
            var result = await _recommendationService.GetSimilarJobsAsync(jobId, count);
            return Ok(new ResponseDto<IEnumerable<JobWithScoreDto>>(true, "Similar jobs retrieved", result));
        }

        [HttpGet("nearby")]
        public async Task<IActionResult> GetNearbyJobs([FromQuery] double lat, [FromQuery] double lng, [FromQuery] double radius = 50, [FromQuery] int count = 20)
        {
            var result = await _recommendationService.GetNearbyJobsAsync(lat, lng, radius, count);
            return Ok(new ResponseDto<IEnumerable<JobListingDto>>(true, "Nearby jobs retrieved", result));
        }
    }
}
