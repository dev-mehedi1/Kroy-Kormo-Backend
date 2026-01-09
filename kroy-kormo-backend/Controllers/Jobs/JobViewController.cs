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
    public class JobViewController : ControllerBase
    {
        private readonly IJobViewService _jobViewService;

        public JobViewController(IJobViewService jobViewService)
        {
            _jobViewService = jobViewService;
        }

        [HttpPost]
        public async Task<IActionResult> TrackView([FromBody] CreateJobViewDto dto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            Guid? userId = string.IsNullOrEmpty(userIdStr) ? null : Guid.Parse(userIdStr);
            
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var userAgent = Request.Headers["User-Agent"].ToString();

            var tracked = await _jobViewService.TrackViewAsync(userId, ipAddress, userAgent, dto);
            
            if (tracked)
                return Ok(new ResponseDto<object>(true, "Job view tracked", null!));
            
            return Ok(new ResponseDto<object>(true, "View already tracked recently", null!));
        }

        [HttpGet("{jobId}/analytics")]
        public async Task<IActionResult> GetJobAnalytics(Guid jobId)
        {
            var analytics = await _jobViewService.GetJobViewsAnalyticsAsync(jobId);
            return Ok(new ResponseDto<JobViewAnalyticsDto>(true, "Job analytics retrieved", analytics));
        }

        [Authorize]
        [HttpGet("history")]
        public async Task<IActionResult> GetMyViewHistory([FromQuery] int count = 20)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            var userId = Guid.Parse(userIdStr);

            var history = await _jobViewService.GetUserViewHistoryAsync(userId, count);
            return Ok(new ResponseDto<IEnumerable<JobViewDto>>(true, "User view history retrieved", history));
        }
    }
}
