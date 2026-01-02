using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Admin;
using Sob_Need_Server.DTOs.Authentication;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.Interface.Admin;
using Sob_Need_Server.Interface.Authentication;

namespace Sob_Need_Server.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly IUserService _userService;
        private readonly AppDbContext _context;

        public AdminService(IUserService userService, AppDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        public async Task<ResponseDto<UserDto>> AdminLoginAsync(string userName, string password)
        {
            var response = await _userService.LoginAsync(userName, password);
            if (!response.IsSuccess) return response;

            // Check if user is Admin
            // Assuming "Admin" or "SuperAdmin" is the slug or TypeName. 
            // Case-insensitive check is safer.
            var userType = response.Data.UserTypeName;
            if (string.IsNullOrEmpty(userType) || 
               (!userType.Equals("Admin", StringComparison.OrdinalIgnoreCase) && 
                !userType.Equals("SuperAdmin", StringComparison.OrdinalIgnoreCase)))
            {
                return new ResponseDto<UserDto>(false, "Access Denied: You do not have Admin privileges.", null!);
            }

            return response;
        }

        public async Task<ResponseDto<AdminDashboardStatsDto>> GetDashboardStatsAsync()
        {
            try
            {
                var totalUsers = await _context.Users.CountAsync();
                var totalActiveAds = await _context.Products.CountAsync(p => p.IsActive);
                // Assuming you might have a status for Pending, for now let's say all inactive are pending/other
                // Or better, check specific status if available. 
                // Based on previous work, we know there's a Status table.
                
                var pendingStatusId = await _context.ProductStatuses
                    .Where(s => s.Slug == "pending")
                    .Select(s => s.StatusId)
                    .FirstOrDefaultAsync();

                var totalPendingAds = pendingStatusId != Guid.Empty 
                    ? await _context.Products.CountAsync(p => p.StatusId == pendingStatusId)
                    : 0;

                var stats = new AdminDashboardStatsDto
                {
                    TotalUsers = totalUsers,
                    TotalActiveAds = totalActiveAds,
                    TotalPendingAds = totalPendingAds
                };

                return new ResponseDto<AdminDashboardStatsDto>(true, "Stats retrieved", stats);
            }
            catch (Exception ex)
            {
                return new ResponseDto<AdminDashboardStatsDto>(false, ex.Message, null!);
            }
        }
    }
}
