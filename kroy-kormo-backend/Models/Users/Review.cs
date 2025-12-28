using Models.Products;
using Sob_Need_Server.Models.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Users
{
    public class Review
    {
        [Key]
        public Guid ReviewId { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid SellerId { get; set; } // User being reviewed
        
        [Required]
        public Guid ReviewerId { get; set; } // User writing the review
        
        public Guid? ProductId { get; set; } // Optional: review related to specific product
        
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } // 1-5 stars
        
        [MaxLength(1000)]
        public string? Comment { get; set; }
        
        [Required]
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual User? Seller { get; set; }
        public virtual User? Reviewer { get; set; }
        public virtual Product? Product { get; set; }
    }
}
