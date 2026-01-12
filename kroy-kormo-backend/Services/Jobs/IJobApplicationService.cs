using Sob_Need_Server.DTOs.Jobs;
using Sob_Need_Server.DTOs.Common;


namespace Sob_Need_Server.Services.Jobs
{
    public interface IJobApplicationService
    {
        // Application Lifecycle
        Task<JobApplicationDto> ApplyForJobAsync(Guid applicantId, CreateApplicationDto dto);
        Task<bool> WithdrawApplicationAsync(Guid applicationId, Guid applicantId);
        
        // Employer Actions
        Task<bool> UpdateApplicationStatusAsync(Guid applicationId, Guid employerId, Guid statusId, string? notes);
        Task<PaginatedResponseDto<JobApplicationDto>> GetJobApplicationsAsync(Guid jobId, Guid employerId, ApplicationFilterDto filter);
        Task<JobApplicationDto?> GetApplicationDetailAsync(Guid applicationId, Guid userId);
        
        // Applicant Actions
        Task<PaginatedResponseDto<JobApplicationDto>> GetUserApplicationsAsync(Guid applicantId, ApplicationFilterDto filter);

        
        // History & Analytics
        Task<IEnumerable<ApplicationStatusHistoryDto>> GetApplicationHistoryAsync(Guid applicationId);
    }
}
