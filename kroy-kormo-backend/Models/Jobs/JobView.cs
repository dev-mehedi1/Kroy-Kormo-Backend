using Sob_Need_Server.Models.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Jobs
{
    public class JobView
    {
        [Key]
        public Guid ViewId { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid JobId { get; set; }
        
        public Guid? UserId { get; set; } // Null for anonymous views
        
        [MaxLength(45)]
        public string? IpAddress { get; set; }
        
        [MaxLength(500)]
        public string? UserAgent { get; set; }
        
        [MaxLength(1000)]
        public string? ReferrerUrl { get; set; } // Where they came from
        
        [MaxLength(100)]
        public string? DeviceType { get; set; } // Mobile, Desktop, Tablet
        
        [MaxLength(100)]
        public string? Country { get; set; }
        
        [MaxLength(100)]
        public string? City { get; set; }
        
        public int? ViewDurationSeconds { get; set; } // How long they viewed the job
        
        public DateTime ViewedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual JobListing? Job { get; set; }
        public virtual User? User { get; set; }
    }
}
