using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Jobs
{
    public class ExperienceLevel
    {
        [Key]
        public Guid ExperienceLevelId { get; set; } = Guid.NewGuid();
        
        [Required]
        [MaxLength(50)]
        public string LevelName { get; set; } = null!; // Entry, Mid, Senior, Executive
        
        [Required]
        [MaxLength(50)]
        public string Slug { get; set; } = null!;
        
        [MaxLength(200)]
        public string? Description { get; set; }
        
        public int MinYears { get; set; } = 0;
        
        public int? MaxYears { get; set; }
        
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
