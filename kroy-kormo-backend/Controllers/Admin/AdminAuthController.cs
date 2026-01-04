using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.Authentication;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.Interface.Admin;
using Sob_Need_Server.Utils;
// Import for seeding logic directly here for convenience or add to Service
using Sob_Need_Server.Utils.Helpers; // For PasswordHelper if needed
using Sob_Need_Server.Models.Authentication;
using Sob_Need_Server.Context;

namespace Sob_Need_Server.Controllers.Admin
{
    [Route(Constants.BasePath + "/admin")]
    [ApiController]
    public class AdminAuthController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly AppDbContext _context; // Inject context for seeding

        public AdminAuthController(IAdminService adminService, AppDbContext context)
        {
            _adminService = adminService;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseDto<UserDto>>> AdminLogin([FromBody] LoginDto loginDto)
        {
            // Updated to use DTO for cleaner API
            // Check if user sent "userName" and "password" as json
            var response = await _adminService.AdminLoginAsync(loginDto.UserName, loginDto.Password);
            if (response.IsSuccess) return Ok(response);
            return BadRequest(response);
        }

        // Temporary endpoint to create first admin
        [HttpPost("seed-admin")]
        public async Task<IActionResult> SeedAdmin([FromBody] AdminUser admin)
        {
            if (_context.AdminUsers.Any(a => a.UserName == admin.UserName))
                return BadRequest("Admin already exists");

            // Hash password
            admin.Password = PasswordHelper.GenerateHashedPassword(admin.Password);
            admin.Role = "SuperAdmin";
            admin.IsActive = true;
            admin.CreatedAt = DateTime.UtcNow;

            await _context.AdminUsers.AddAsync(admin);
            await _context.SaveChangesAsync();

            return Ok("Admin user seeded successfully");
        }
    }

    public class LoginDto {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
