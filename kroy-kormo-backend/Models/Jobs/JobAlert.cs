using Sob_Need_Server.Models.Authentication;
using Sob_Need_Server.Models.Categories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sob_Need_Server.Models.Jobs
{
    public class JobAlert
    {
        [Key]
        public Guid AlertId { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string AlertName { get; set; } = null!;
        
        // Search Criteria
        [MaxLength(500)]
        public string? Keywords { get; set; }
        
        public Guid? CategoryId { get; set; }
        
        public Guid? LocationId { get; set; }
        
        public Guid? JobTypeId { get; set; }
        
        public Guid? ExperienceLevelId { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinSalary { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxSalary { get; set; }
        
        public bool? IsRemote { get; set; }
        
        public bool? IsHybrid { get; set; }
        
        public Guid? CompanyId { get; set; }
        
        // Alert Settings
        [Required]
        [MaxLength(20)]
        public string Frequency { get; set; } = "Daily"; // Immediate, Daily, Weekly
        
        [Required]
        public bool IsActive { get; set; } = true;
        
        [Required]
        public bool EmailNotification { get; set; } = true;
        
        [Required]
        public bool PushNotification { get; set; } = true;
        
        // Tracking
        public DateTime? LastSentAt { get; set; }
        
        public int MatchCount { get; set; } = 0; // Number of jobs matched since last sent
        
        public DateTime? LastMatchedAt { get; set; }
        
        // Metadata
        [MaxLength(100)]
        public string? CreatedBy { get; set; }
        
        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual User? User { get; set; }
        public virtual Category? Category { get; set; }
        public virtual JobType? JobType { get; set; }
        public virtual ExperienceLevel? ExperienceLevel { get; set; }
    }
}
