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
    [Authorize]
    public class JobInsightController(IJobInsightService jobInsightService) : ControllerBase
    {
        private readonly IJobInsightService _jobInsightService = jobInsightService;

        [HttpGet("my")]
        public async Task<IActionResult> GetMyInsights()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) 
                return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            
            var userId = Guid.Parse(userIdStr);
            var result = await _jobInsightService.GetUserInsightsAsync(userId);
            
            return Ok(new ResponseDto<UserInsightDto>(true, "Job insights retrieved", result));
        }
    }
}
