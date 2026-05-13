using Sob_Need_Server.DTOs.Jobs;

namespace Sob_Need_Server.Services.Jobs
{
    public interface IJobInsightService
    {
        Task<UserInsightDto> GetUserInsightsAsync(Guid userId);
    }
}
