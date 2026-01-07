using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Jobs
{
    public class ApplicationStatus
    {
        [Key]
        public Guid ApplicationStatusId { get; set; } = Guid.NewGuid();
        
        [Required]
        [MaxLength(50)]
        public string StatusName { get; set; } = null!; // Applied, Under Review, Shortlisted, Interview, Rejected, Hired
        
        [Required]
        [MaxLength(50)]
        public string Slug { get; set; } = null!;
        
        [MaxLength(200)]
        public string? Description { get; set; }
        
        [MaxLength(20)]
        public string? ColorCode { get; set; } // For UI display (e.g., "#28a745" for success)
        
        [Required]
        public bool IsActive { get; set; } = true;
        
        [Required]
        public bool IsTerminal { get; set; } = false; // Whether this status ends the application process (e.g., Hired, Rejected)
        
        [MaxLength(100)]
        public string? CreatedBy { get; set; }
        
        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
    }
}
