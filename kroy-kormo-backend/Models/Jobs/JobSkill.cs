using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Jobs
{
    public class JobSkill
    {
        [Key]
        public Guid JobSkillId { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid JobId { get; set; }
        
        [Required]
        public Guid SkillId { get; set; }
        
        [Required]
        public bool IsRequired { get; set; } = true; // Required vs Nice-to-have
        
        public int? MinYearsExperience { get; set; }
        
        [MaxLength(50)]
        public string? ProficiencyLevel { get; set; } // Beginner, Intermediate, Advanced, Expert
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual JobListing? Job { get; set; }
        public virtual Skill? Skill { get; set; }
    }
}
