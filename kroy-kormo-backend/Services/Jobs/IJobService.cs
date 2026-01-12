using Sob_Need_Server.DTOs.Jobs;
using Sob_Need_Server.DTOs.Common;

namespace Sob_Need_Server.Services.Jobs
{
    public interface IJobService
    {
        // Job Listing CRUD
        Task<JobListingDto> CreateJobAsync(Guid employerId, CreateJobListingDto dto);
        Task<JobListingDto> UpdateJobAsync(Guid jobId, Guid employerId, UpdateJobListingDto dto);
        Task<bool> DeleteJobAsync(Guid jobId, Guid employerId);
        Task<JobListingDto?> GetJobByIdAsync(Guid jobId);
        
        // Search and Filtering
        Task<PaginatedResponseDto<JobListingDto>> SearchJobsAsync(JobSearchFilter filter);

        Task<PaginatedResponseDto<JobListingDto>> GetEmployerJobsAsync(Guid employerId, JobSearchFilter filter);
        
        // Job Status & Management
        Task<bool> UpdateJobStatusAsync(Guid jobId, Guid employerId, string status);
        Task<bool> ToggleRemoteStatusAsync(Guid jobId, Guid employerId);
        
        // Metadata / Lookups
        Task<IEnumerable<JobTypeDto>> GetJobTypesAsync();
        Task<IEnumerable<ExperienceLevelDto>> GetExperienceLevelsAsync();
        Task<IEnumerable<SkillDto>> GetSkillsAsync(string? query);
    }

    public class JobSearchFilter
    {
        public string? Keyword { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? JobTypeId { get; set; }
        public Guid? ExperienceLevelId { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public bool? IsRemote { get; set; }
        public string? Location { get; set; }
        
        // Advanced Filters
        public List<Guid>? SkillIds { get; set; }
        public int? DaysAgo { get; set; }
        public Guid? CompanyId { get; set; }
        
        // Geospatial (Placeholder)
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? RadiusKm { get; set; }

        public string? SortBy { get; set; } // "newest", "salary_asc", "salary_desc", "mostviewed", "mostapplied"
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
