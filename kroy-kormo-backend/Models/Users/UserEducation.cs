using Sob_Need_Server.Models.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Users
{
    public class UserEducation
    {
        [Key]
        public Guid EducationId { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Degree { get; set; } = null!; // Bachelor's, Master's, PhD, High School, etc.
        
        [Required]
        [MaxLength(200)]
        public string FieldOfStudy { get; set; } = null!;
        
        [Required]
        [MaxLength(200)]
        public string Institution { get; set; } = null!;
        
        [MaxLength(100)]
        public string? Location { get; set; }
        
        public DateTime? StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        [Required]
        public bool IsCurrent { get; set; } = false;
        
        [MaxLength(50)]
        public string? Grade { get; set; } // GPA, Percentage, Class (First, Second, etc.)
        
        [MaxLength(2000)]
        public string? Description { get; set; } // Activities, achievements, coursework
        
        [MaxLength(500)]
        public string? Achievements { get; set; }
        
        // Display Order
        public int DisplayOrder { get; set; } = 0;
        
        // Metadata
        [Required]
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual User? User { get; set; }
    }
}
