using Sob_Need_Server.Models.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Users
{
    public class UserExperience
    {
        [Key]
        public Guid ExperienceId { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string JobTitle { get; set; } = null!;
        
        [Required]
        [MaxLength(200)]
        public string CompanyName { get; set; } = null!;
        
        public Guid? CompanyId { get; set; } // Link to CompanyProfile if exists in system
        
        [MaxLength(100)]
        public string? Location { get; set; }
        
        [MaxLength(50)]
        public string? EmploymentType { get; set; } // Full-time, Part-time, Contract, Internship
        
        public DateTime? StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        [Required]
        public bool IsCurrent { get; set; } = false;
        
        [MaxLength(5000)]
        public string? Description { get; set; } // Responsibilities and achievements
        
        [MaxLength(2000)]
        public string? KeyAchievements { get; set; }
        
        [MaxLength(1000)]
        public string? Skills { get; set; } // Comma-separated or JSON array of skills used
        
        // Display Order
        public int DisplayOrder { get; set; } = 0;
        
        // Verification
        public bool IsVerified { get; set; } = false;
        
        public DateTime? VerifiedAt { get; set; }
        
        // Metadata
        [Required]
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual User? User { get; set; }
    }
}
