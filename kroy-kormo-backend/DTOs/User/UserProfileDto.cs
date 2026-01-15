namespace Sob_Need_Server.DTOs.User
{
    public class UserEducationDto
    {
        public Guid EducationId { get; set; }
        public string Degree { get; set; } = null!;
        public string FieldOfStudy { get; set; } = null!;
        public string Institution { get; set; } = null!;
        public string? Location { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrent { get; set; }
        public string? Description { get; set; }
        public string? Grade { get; set; }
    }

    public class UserExperienceDto
    {
        public Guid ExperienceId { get; set; }
        public string JobTitle { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        public string? Location { get; set; }
        public string? EmploymentType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrent { get; set; }
        public string? Description { get; set; }
    }

    public class UserSkillDto
    {
        public Guid UserSkillId { get; set; }
        public Guid SkillId { get; set; }
        public string SkillName { get; set; } = null!;
        public int? YearsOfExperience { get; set; }
        public string? ProficiencyLevel { get; set; }
    }

    public class UserResumeDto
    {
        public Guid ResumeId { get; set; }
        public string FileName { get; set; } = null!;
        public string FileUrl { get; set; } = null!;
        public string? FileId { get; set; }
        public string FileType { get; set; } = null!;
        public long FileSize { get; set; }
        public bool IsDefault { get; set; }
        public DateTime UploadedAt { get; set; }
    }

    public class ExtendedProfileDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string? Bio { get; set; }
        public string? AboutMe { get; set; }
        public string? ProfilePicture { get; set; }
        
        // Salary Information
        public decimal? CurrentSalary { get; set; }
        public decimal? ExpectedSalary { get; set; }
        public bool IsSalaryVisible { get; set; }
        
        // Experience Tracking
        public bool IsFresher { get; set; }
        public int? TotalExperienceMonths { get; set; }
        
        public List<UserEducationDto> Educations { get; set; } = new();
        public List<UserExperienceDto> Experiences { get; set; } = new();
        public List<UserSkillDto> Skills { get; set; } = new();
        public List<UserResumeDto> Resumes { get; set; } = new();
    }

    public class UpdateProfileDto
    {
        public string? Bio { get; set; }
        public string? AboutMe { get; set; }
        public decimal? CurrentSalary { get; set; }
        public decimal? ExpectedSalary { get; set; }
        public bool? IsSalaryVisible { get; set; }
        public bool? IsFresher { get; set; }
    }
}
