using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Admin;
using Sob_Need_Server.DTOs.Authentication;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.Interface.Admin;
using Sob_Need_Server.Interface.Authentication;
using Sob_Need_Server.Utils.Helpers;
using Sob_Need_Server.Models.Authentication;
using Sob_Need_Server.Models.Products;
using System.Linq;

namespace Sob_Need_Server.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;
        private readonly TokenHelper _tokenHelper;

        public AdminService(AppDbContext context, TokenHelper tokenHelper)
        {
            _context = context;
            _tokenHelper = tokenHelper;
        }

        public async Task<ResponseDto<UserDto>> AdminLoginAsync(string userName, string password)
        {
            try
            {
                var adminUser = await _context.AdminUsers.FirstOrDefaultAsync(x => x.UserName == userName);
                if (adminUser == null) return new ResponseDto<UserDto>(false, "Admin not found", null!);

                var isPasswordValid = PasswordHelper.VerifyPassword(adminUser.Password, password);
                if (!isPasswordValid) return new ResponseDto<UserDto>(false, "Invalid password", null!);

                if (!adminUser.IsActive) return new ResponseDto<UserDto>(false, "Admin account is inactive", null!);

                // Generate JWT using TokenHelper
                // Map AdminUser to User entity because TokenHelper expects User
                var userForToken = new Sob_Need_Server.Models.Authentication.User
                {
                    UserId = adminUser.AdminId,
                    UserName = adminUser.UserName,
                    Email = adminUser.Email,
                    FullName = adminUser.FullName,
                    UserType = new Sob_Need_Server.Models.Authentication.UserType { TypeName = adminUser.Role }
                };

                var token = _tokenHelper.GenerateJwtToken(userForToken);
                
                // Construct UserDto
                var userDto = new UserDto
                {
                    UserId = adminUser.AdminId,
                    UserName = adminUser.UserName,
                    FullName = adminUser.FullName,
                    Email = adminUser.Email,
                    Phone = adminUser.Phone,
                    UserTypeName = adminUser.Role,
                    IsActive = adminUser.IsActive,
                    CreatedAt = adminUser.CreatedAt,
                    Token = token
                };

                // Update LastLogin
                adminUser.LastLoginAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return new ResponseDto<UserDto>(true, "Admin Login successful", userDto);
            }
            catch (Exception ex)
            {
                return new ResponseDto<UserDto>(false, ex.Message, null!);
            }
        }

        public async Task<ResponseDto<AdminDashboardStatsDto>> GetDashboardStatsAsync()
        {
            try
            {
                var totalUsers = await _context.Users.CountAsync();
                var totalActiveAds = await _context.Products.CountAsync(p => p.IsActive);
                
                // Fetch pending status ID first
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
