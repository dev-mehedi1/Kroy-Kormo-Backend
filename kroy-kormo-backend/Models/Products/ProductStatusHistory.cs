using System.ComponentModel.DataAnnotations;
using Models.Products;

namespace Sob_Need_Server.Models.Products
{
    public class ProductStatusHistory
    {
        [Key]
        public Guid HistoryId { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid ProductId { get; set; }
        
        [Required]
        public Guid StatusId { get; set; }
        
        [MaxLength(500)]
        public string? Notes { get; set; }
        
        [MaxLength(100)]
        public string? ChangedBy { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual Product? Product { get; set; }
        public virtual ProductStatus? Status { get; set; }
    }
}
