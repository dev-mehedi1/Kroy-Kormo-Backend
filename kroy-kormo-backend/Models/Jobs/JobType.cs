using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sob_Need_Server.Models.Jobs
{
    public class JobType
    {
        [Key]
        public Guid JobTypeId { get; set; } = Guid.NewGuid();
        
        [Required]
        [MaxLength(50)]
        public string TypeName { get; set; } = null!; // Full-time, Part-time, Contract, Internship, Remote
        
        [Required]
        [MaxLength(50)]
        public string Slug { get; set; } = null!;
        
        [MaxLength(200)]
        public string? Description { get; set; }
        
        [Required]
        public bool IsActive { get; set; } = true;
        
        [MaxLength(100)]
        public string? CreatedBy { get; set; }
        
        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<JobListing> JobListings { get; set; } = new List<JobListing>();
    }
}
