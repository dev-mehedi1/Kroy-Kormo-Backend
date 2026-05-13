using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.Interface.Analytics;
using Sob_Need_Server.Utils;

namespace Sob_Need_Server.Controllers.Analytics
{
    [Route(Constants.BasePath + "/analytics")]
    [ApiController]
    public class AnalyticsController(IAnalyticsService analyticsService) : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService = analyticsService;

        [HttpGet("dashboard-stats")]
        [Authorize(Roles = "SuperAdmin,Admin,Owner")]
        public async Task<IActionResult> GetDashboardStats()
        {
            // Assuming IAnalyticsService has a method for this.
            // If not, I might need to update the service interface/impl as well.
            // For now, I will create a placeholder response or update service next.
             var response = await _analyticsService.GetDashboardStatsAsync();
             return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("seller-stats")]
        [Authorize]
        public async Task<IActionResult> GetSellerStats()
        {
            var userIdStr = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdStr)) 
                return Unauthorized(new ResponseDto<object>(false, "User not authenticated", null!));
            
            var userId = Guid.Parse(userIdStr);
            var response = await _analyticsService.GetSellerStatsAsync(userId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
