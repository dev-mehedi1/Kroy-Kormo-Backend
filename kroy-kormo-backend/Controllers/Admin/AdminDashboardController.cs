using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.Admin;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.Interface.Admin;
using Sob_Need_Server.Utils;

namespace Sob_Need_Server.Controllers.Admin
{
    [Route(Constants.BasePath + "/admin/dashboard")]
    [ApiController]
    // [Authorize] // Uncomment when ready to enforce token check on stats
    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminDashboardController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("stats")]
        public async Task<ActionResult<ResponseDto<AdminDashboardStatsDto>>> GetStats()
        {
            var response = await _adminService.GetDashboardStatsAsync();
            if (response.IsSuccess) return Ok(response);
            return BadRequest(response);
        }
    }
}
