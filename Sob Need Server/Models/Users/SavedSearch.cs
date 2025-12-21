using Sob_Need_Server.Models.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Users
{
    public class SavedSearch
    {
        public Guid SearchId { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string SearchName { get; set; } = null!;
        
        [Required]
        public string FilterJson { get; set; } = null!; // Serialized filter criteria
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual User? User { get; set; }
    }
}
