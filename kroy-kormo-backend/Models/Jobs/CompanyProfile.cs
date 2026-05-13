using Sob_Need_Server.Models.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Jobs
{
    public class CompanyProfile
    {
        [Key]
        public Guid CompanyId { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid UserId { get; set; } // Owner/Creator
        
        [Required]
        [MaxLength(200)]
        public string CompanyName { get; set; } = null!;
        
        [MaxLength(250)]
        public string? Slug { get; set; }
        
        [MaxLength(5000)]
        public string? Description { get; set; }
        
        [MaxLength(100)]
        public string? Industry { get; set; }
        
        [MaxLength(50)]
        public string? CompanySize { get; set; } // 1-10, 11-50, 51-200, 201-500, 500+
        
        [MaxLength(200)]
        public string? Website { get; set; }
        
        [MaxLength(500)]
        public string? LogoUrl { get; set; }
        
        [MaxLength(500)]
        public string? CoverImageUrl { get; set; }
        
        // Contact Information
        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }
        
        [MaxLength(20)]
        public string? Phone { get; set; }
        
        [MaxLength(500)]
        public string? Address { get; set; }
        
        // Verification
        [Required]
        public bool IsVerified { get; set; } = false;
        
        public DateTime? VerifiedAt { get; set; }
        
        public Guid? VerifiedBy { get; set; }
        
        // Social Media
        [MaxLength(200)]
        public string? LinkedInUrl { get; set; }
        
        [MaxLength(200)]
        public string? FacebookUrl { get; set; }
        
        [MaxLength(200)]
        public string? TwitterUrl { get; set; }
        
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
        public virtual User? Owner { get; set; }
        public virtual ICollection<JobListing> JobListings { get; set; } = new List<JobListing>();
    }
}
