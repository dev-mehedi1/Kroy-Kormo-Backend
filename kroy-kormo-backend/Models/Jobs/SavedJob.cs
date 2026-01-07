using Sob_Need_Server.Models.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Jobs
{
    public class SavedJob
    {
        [Key]
        public Guid SavedJobId { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        public Guid JobId { get; set; }
        
        [MaxLength(500)]
        public string? Notes { get; set; } // User's personal notes about the job
        
        public DateTime SavedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual User? User { get; set; }
        public virtual JobListing? Job { get; set; }
    }
}
