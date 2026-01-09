using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.Interface.Analytics;
using Sob_Need_Server.Utils;

namespace Sob_Need_Server.Controllers.Analytics
{
    [Route(Constants.BasePath + "/analytics")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Admin,Owner")]
    public class AnalyticsController(IAnalyticsService analyticsService) : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService = analyticsService;

        [HttpGet("dashboard-stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            // Assuming IAnalyticsService has a method for this.
            // If not, I might need to update the service interface/impl as well.
            // For now, I will create a placeholder response or update service next.
             var response = await _analyticsService.GetDashboardStatsAsync();
             return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
