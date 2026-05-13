using Sob_Need_Server.Models.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Users
{
    public class UserResume
    {
        [Key]
        public Guid ResumeId { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        [MaxLength(1000)]
        public string ResumeUrl { get; set; } = null!;
        
        [MaxLength(100)]
        public string? ResumeTitle { get; set; } // User-defined title (e.g., "Software Engineer Resume")
        
        [Required]
        public bool IsDefault { get; set; } = false;
        
        [Required]
        public bool IsActive { get; set; } = true;
        
        // Parsed Content (optional - for search/matching)
        public string? ParsedContent { get; set; } // Text extraction for keyword matching
        
        // Usage tracking
        public int TimesUsed { get; set; } = 0; // How many times applied with this resume
        
        public DateTime? LastUsedAt { get; set; }
        
        
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual User? User { get; set; }
    }
}
