using Sob_Need_Server.DTOs.Jobs;

namespace Sob_Need_Server.Services.Jobs
{
    public interface IJobAlertService
    {
        Task<JobAlertDto> CreateAlertAsync(Guid userId, CreateJobAlertDto dto);
        Task<JobAlertDto> UpdateAlertAsync(Guid alertId, Guid userId, UpdateJobAlertDto dto);
        Task<bool> DeleteAlertAsync(Guid alertId, Guid userId);
        Task<IEnumerable<JobAlertDto>> GetUserAlertsAsync(Guid userId);
        Task<JobAlertDto?> GetAlertByIdAsync(Guid alertId, Guid userId);
        Task<bool> ToggleAlertStatusAsync(Guid alertId, Guid userId);
        Task<IEnumerable<JobListingDto>> GetAlertMatchesAsync(Guid alertId, Guid userId);
        Task<int> CheckAlertsForNewJobAsync(JobListingDto job);
    }
}
