using Sob_Need_Server.DTOs.Authentication;
using Sob_Need_Server.DTOs.Common;

namespace Sob_Need_Server.Interface.Admin
{
    public interface IAdminService
    {
        Task<ResponseDto<UserDto>> AdminLoginAsync(string userName, string password);
        Task<ResponseDto<DTOs.Admin.AdminDashboardStatsDto>> GetDashboardStatsAsync();
    }
}
