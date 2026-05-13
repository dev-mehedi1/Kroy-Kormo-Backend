using Sob_Need_Server.DTOs.Jobs;

namespace Sob_Need_Server.Services.Jobs
{
    public interface IJobViewService
    {
        Task<bool> TrackViewAsync(Guid? userId, string ipAddress, string userAgent, CreateJobViewDto dto);
        Task<JobViewAnalyticsDto> GetJobViewsAnalyticsAsync(Guid jobId, int days = 30);
        Task<IEnumerable<JobViewDto>> GetUserViewHistoryAsync(Guid userId, int count = 20);
    }
}
