using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Authentication;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.Utils;

namespace Sob_Need_Server.Controllers.Admin
{
    [Route(Constants.BasePath + "/admin/users")]
    [ApiController]

    public class AdminUserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminUserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<UserDto>>>> GetUsers()
        {
            try
            {
               // Simple implementation: Get all users
               // Ideally, implement pagination and filtering
               var users = await _context.Users
                   .Include(u => u.UserType)
                   .Select(u => new UserDto
                   {
                       UserId = u.UserId,
                       UserName = u.UserName,
                       FullName = u.FullName,
                       Email = u.Email,
                       Phone = u.Phone,
                       UserTypeName = u.UserType != null ? u.UserType.TypeName : "Unknown",
                       IsActive = u.IsActive,
                       IsVerified = u.IsVerified,
                       CreatedAt = u.CreatedAt
                   })
                   .ToListAsync();

                return Ok(new ResponseDto<IEnumerable<UserDto>>(true, "Users retrieved", users));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto<IEnumerable<UserDto>>(false, ex.Message, null!));
            }
        }
        
        [HttpPatch("{userId}/status")]
         public async Task<ActionResult<ResponseDto<bool>>> UpdateUserStatus(Guid userId, [FromBody] bool isActive)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) return NotFound(new ResponseDto<bool>(false, "User not found", false));

                user.IsActive = isActive;
                await _context.SaveChangesAsync();

                return Ok(new ResponseDto<bool>(true, $"User status updated to {(isActive ? "Active" : "Inactive")}", true));
            }
             catch (Exception ex)
            {
                return BadRequest(new ResponseDto<bool>(false, ex.Message, false));
            }
        }
    }
}
