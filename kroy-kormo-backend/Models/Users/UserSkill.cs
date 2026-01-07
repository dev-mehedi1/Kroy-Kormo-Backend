using Sob_Need_Server.Models.Authentication;
using Sob_Need_Server.Models.Jobs;
using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Users
{
    public class UserSkill
    {
        [Key]
        public Guid UserSkillId { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        public Guid SkillId { get; set; }
        
        public int? YearsOfExperience { get; set; }
        
        [MaxLength(50)]
        public string? ProficiencyLevel { get; set; } // Beginner, Intermediate, Advanced, Expert
        
        [MaxLength(500)]
        public string? Notes { get; set; } // Additional context about the skill
        
        // Endorsements
        public int EndorsementCount { get; set; } = 0;
        
        // Verification (e.g., through certification or test)
        public bool IsVerified { get; set; } = false;
        
        public DateTime? VerifiedAt { get; set; }
        
        [MaxLength(200)]
        public string? VerificationSource { get; set; } // LinkedIn, Certification, Test
        
        // Featured
        public bool IsFeatured { get; set; } = false; // Top skills to highlight
        
        public int DisplayOrder { get; set; } = 0;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual User? User { get; set; }
        public virtual Skill? Skill { get; set; }
    }
}
