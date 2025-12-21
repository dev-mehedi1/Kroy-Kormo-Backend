using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Products
{
    public class ProductPromotion
    {
        public Guid PromotionId { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid ProductId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string PromotionType { get; set; } = null!; // TopAd, BumpUp, Urgent, Featured
        
        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
        
        [Required]
        public bool IsActive { get; set; } = true;
        
        [MaxLength(100)]
        public string? CreatedBy { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual Product? Product { get; set; }
    }
}
