using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Jobs
{
    public class ApplicationStatusHistory
    {
        [Key]
        public Guid HistoryId { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid ApplicationId { get; set; }
        
        [Required]
        public Guid StatusId { get; set; }
        
        [MaxLength(1000)]
        public string? Notes { get; set; }
        
        public Guid? ChangedBy { get; set; } // User who changed the status (employer)
        
        [MaxLength(100)]
        public string? ChangedByRole { get; set; } // Employer, System, Applicant
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual JobApplication? Application { get; set; }
        public virtual ApplicationStatus? Status { get; set; }
    }
}
