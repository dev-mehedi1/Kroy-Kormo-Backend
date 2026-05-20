using Models.Products;
using Sob_Need_Server.Models.Authentication;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sob_Need_Server.Models.Jobs
{
    public class JobListing
    {
        [Key]
        public Guid JobId { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid ProductId { get; set; } // Links to Products table (for unified system)
        
        [Required]
        public Guid EmployerId { get; set; } // UserId of employer
        
        public Guid? CompanyProfileId { get; set; }
        
        [Required]
        public Guid JobTypeId { get; set; }
        
        [Required]
        public Guid ExperienceLevelId { get; set; }
        
        [Required]
        public Guid CategoryId { get; set; } // Link to Category table (Jobs category)

        
        // Salary Information
        [Column(TypeName = "decimal(18,2)")]
        public decimal? SalaryMin { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? SalaryMax { get; set; }
        
        [MaxLength(10)]
        public string? SalaryCurrency { get; set; } // USD, EUR, BDT, etc.
        
        [MaxLength(20)]
        public string? SalaryPeriod { get; set; } // Hourly, Daily, Monthly, Yearly
        
        [Required]
        public bool ShowSalary { get; set; } = true;
        
        // Job Details
        public int VacancyCount { get; set; } = 1;
        
        public DateTime? ApplicationDeadline { get; set; }
        
        [MaxLength(5000)]
        public string? Requirements { get; set; } // Required qualifications
        
        [MaxLength(5000)]
        public string? Responsibilities { get; set; } // Job responsibilities
        
        [MaxLength(3000)]
        public string? Benefits { get; set; } // Perks and benefits
        
        [MaxLength(2000)]
        public string? PreferredQualifications { get; set; }
        
        // Work Arrangement
        [Required]
        public bool IsRemote { get; set; } = false;
        
        [Required]
        public bool IsHybrid { get; set; } = false;
        
        [MaxLength(100)]
        public string? WorkSchedule { get; set; } // e.g., "Monday-Friday, 9AM-5PM"
        
        [MaxLength(200)]
        public string? OfficeLocation { get; set; }
        
        // Application Settings
        [MaxLength(500)]
        public string? ExternalApplyUrl { get; set; }
        
        [Required]
        public bool RequireResume { get; set; } = true;
        
        [Required]
        public bool RequireCoverLetter { get; set; } = false;
        
        public int ApplicationCount { get; set; } = 0;
        
        public int ViewCount { get; set; } = 0;
        
        // Job Status
        [MaxLength(50)]
        public string JobStatus { get; set; } = "Active"; // Active, Paused, Filled, Expired, Closed
        
        public DateTime? FilledAt { get; set; }
        
        public DateTime? ClosedAt { get; set; }
        
        // Metadata
        [Required]
        public bool IsActive { get; set; } = true;
        
        [MaxLength(100)]
        public string? CreatedBy { get; set; }
        
        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual Product? Product { get; set; }
        public virtual User? Employer { get; set; }
        public virtual JobType? JobType { get; set; }
        public virtual ExperienceLevel? ExperienceLevel { get; set; }
        public virtual Sob_Need_Server.Models.Categories.Category? Category { get; set; }
        public virtual CompanyProfile? CompanyProfile { get; set; }

        public virtual ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
        public virtual ICollection<JobSkill> RequiredSkills { get; set; } = new List<JobSkill>();
        public virtual ICollection<SavedJob> SavedByUsers { get; set; } = new List<SavedJob>();
        public virtual ICollection<JobView> Views { get; set; } = new List<JobView>();
    }
}
