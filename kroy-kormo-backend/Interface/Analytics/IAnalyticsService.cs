using System;
using System.Threading.Tasks;
using Sob_Need_Server.DTOs.Analytics;
using Sob_Need_Server.DTOs.Common;

namespace Sob_Need_Server.Interface.Analytics
{
    public interface IAnalyticsService
    {
        Task<ResponseDto<SellerStatsDto>> GetSellerStatsAsync(Guid userId);
        Task<ResponseDto<bool>> TrackProductViewAsync(Guid productId, Guid? userId, string ipAddress, string userAgent);
        Task<ResponseDto<DashboardStatsDto>> GetDashboardStatsAsync();
    }
}
