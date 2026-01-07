using Sob_Need_Server.DTOs.Jobs;

namespace Sob_Need_Server.Services.Jobs
{
    public interface IJobApplicationService
    {
        // Application Lifecycle
        Task<JobApplicationDto> ApplyForJobAsync(Guid applicantId, CreateApplicationDto dto);
        Task<bool> WithdrawApplicationAsync(Guid applicationId, Guid applicantId);
        
        // Employer Actions
        Task<bool> UpdateApplicationStatusAsync(Guid applicationId, Guid employerId, Guid statusId, string? notes);
        Task<IEnumerable<JobApplicationDto>> GetJobApplicationsAsync(Guid jobId, Guid employerId);
        Task<JobApplicationDto?> GetApplicationDetailAsync(Guid applicationId, Guid userId);
        
        // Applicant Actions
        Task<IEnumerable<JobApplicationDto>> GetUserApplicationsAsync(Guid applicantId);
        
        // History & Analytics
        Task<IEnumerable<ApplicationStatusHistoryDto>> GetApplicationHistoryAsync(Guid applicationId);
    }
}
