using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Authentication
{
    public class UserType
    {
        [Key]
        public Guid UserTypeId { get; set; } = Guid.NewGuid();
        
        [Required]
        [MaxLength(50)]
        public string TypeName { get; set; } = null!;
        
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
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
