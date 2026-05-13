using System.ComponentModel.DataAnnotations;
using Sob_Need_Server.Models.Users;


namespace Sob_Need_Server.Models.Jobs
{
    public class Skill
    {
        [Key]
        public Guid SkillId { get; set; } = Guid.NewGuid();
        
        [Required]
        [MaxLength(100)]
        public string SkillName { get; set; } = null!;
        
        [Required]
        [MaxLength(100)]
        public string Slug { get; set; } = null!;
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        public Guid? CategoryId { get; set; } // Optional: Programming, Design, Management, etc.
        
        [Required]
        public bool IsActive { get; set; } = true;
        
        [MaxLength(100)]
        public string? CreatedBy { get; set; }
        
        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();
        public virtual ICollection<JobSkill> JobSkills { get; set; } = new List<JobSkill>();
    }
}
