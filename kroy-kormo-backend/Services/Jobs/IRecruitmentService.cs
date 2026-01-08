using Sob_Need_Server.DTOs.Jobs;

namespace Sob_Need_Server.Services.Jobs
{
    public interface IRecruitmentService
    {
        // Company Profile
        Task<CompanyProfileDto> CreateCompanyProfileAsync(Guid userId, CompanyProfileDto dto);
        Task<CompanyProfileDto> UpdateCompanyProfileAsync(Guid userId, CompanyProfileDto dto);
        Task<CompanyProfileDto?> GetCompanyProfileAsync(Guid companyId);
        Task<IEnumerable<CompanyProfileDto>> GetUserCompanyProfilesAsync(Guid userId);
        
        // Interviews
        Task<InterviewDto> ScheduleInterviewAsync(Guid employerId, InterviewDto dto);
        Task<bool> UpdateInterviewStatusAsync(Guid interviewId, Guid userId, string status);
        Task<IEnumerable<InterviewDto>> GetJobInterviewsAsync(Guid jobId, Guid employerId);
        
        // Offers
        Task<JobOfferDto> CreateOfferAsync(Guid employerId, JobOfferDto dto);
        Task<bool> RespondToOfferAsync(Guid offerId, Guid applicantId, string response, string? notes);
        
        // Analytics
        Task<JobStatisticsDto> GetJobStatisticsAsync(Guid jobId, Guid employerId);
    }
}
