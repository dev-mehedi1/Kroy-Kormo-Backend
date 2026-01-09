namespace Sob_Need_Server.DTOs.Jobs
{
    public class JobTypeDto
    {
        public Guid JobTypeId { get; set; }
        public string TypeName { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Description { get; set; }
    }

    public class ExperienceLevelDto
    {
        public Guid ExperienceLevelId { get; set; }
        public string LevelName { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public int MinYears { get; set; }
        public int? MaxYears { get; set; }
    }



    public class JobListingDto
    {
        public Guid JobId { get; set; }
        public Guid ProductId { get; set; }
        public Guid EmployerId { get; set; }
        public string EmployerName { get; set; } = null!;
        
        public Guid? CompanyProfileId { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyLogoUrl { get; set; }
        
        public Guid JobTypeId { get; set; }
        public string JobTypeName { get; set; } = null!;
        
        public Guid ExperienceLevelId { get; set; }
        public string ExperienceLevelName { get; set; } = null!;
        
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public string? SalaryCurrency { get; set; }
        public string? SalaryPeriod { get; set; }
        public bool ShowSalary { get; set; }
        
        public int VacancyCount { get; set; }
        public DateTime? ApplicationDeadline { get; set; }
        
        public string? Requirements { get; set; }
        public string? Responsibilities { get; set; }
        public string? Benefits { get; set; }
        
        public bool IsRemote { get; set; }
        public bool IsHybrid { get; set; }
        public string? WorkSchedule { get; set; }
        public string? OfficeLocation { get; set; }
        
        public string JobStatus { get; set; } = null!;
        public int ApplicationCount { get; set; }
        public int ViewCount { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        // From Product
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Slug { get; set; } = null!;
        
        public List<SkillDto> RequiredSkills { get; set; } = new();
    }

    public class CreateJobListingDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid CategoryId { get; set; }
        
        public Guid? CompanyProfileId { get; set; }
        public Guid JobTypeId { get; set; }
        public Guid ExperienceLevelId { get; set; }
        
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public string? SalaryCurrency { get; set; } = "USD";
        public string? SalaryPeriod { get; set; } = "Monthly";
        public bool ShowSalary { get; set; } = true;
        
        public int VacancyCount { get; set; } = 1;
        public DateTime? ApplicationDeadline { get; set; }
        
        public string? Requirements { get; set; }
        public string? Responsibilities { get; set; }
        public string? Benefits { get; set; }
        
        public bool IsRemote { get; set; }
        public bool IsHybrid { get; set; }
        public string? WorkSchedule { get; set; }
        public string? OfficeLocation { get; set; }
        
        public bool RequireResume { get; set; } = true;
        public bool RequireCoverLetter { get; set; } = false;
        
        public List<Guid> SkillIds { get; set; } = new();
    }

    public class UpdateJobListingDto : CreateJobListingDto
    {
        public string JobStatus { get; set; } = "Active";
    }
}
