using Sob_Need_Server.Models.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Products
{
    public class ProductView
    {
        public Guid ViewId { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid ProductId { get; set; }
        
        public Guid? UserId { get; set; } // Nullable for anonymous views
        
        [MaxLength(45)]
        public string? IpAddress { get; set; }
        
        [MaxLength(500)]
        public string? UserAgent { get; set; }
        
        public DateTime ViewedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual Product? Product { get; set; }
        public virtual User? User { get; set; }
    }
}
