using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.Authentication;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.Interface.Admin;
using Sob_Need_Server.Utils;

namespace Sob_Need_Server.Controllers.Admin
{
    [Route(Constants.BasePath + "/admin")]
    [ApiController]
    public class AdminAuthController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminAuthController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseDto<UserDto>>> AdminLogin(string userName, string password)
        {
            var response = await _adminService.AdminLoginAsync(userName, password);
            if (response.IsSuccess) return Ok(response);
            return BadRequest(response);
        }
    }
}
