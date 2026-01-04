using Models.Products;
using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Products
{
    public class ProductAttributeValue
    {
        [Key]
        public Guid ValueId { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid ProductId { get; set; }
        
        [Required]
        public Guid AttributeId { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string Value { get; set; } = null!;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual Product? Product { get; set; }
        public virtual ProductAttribute? Attribute { get; set; }
    }
}
