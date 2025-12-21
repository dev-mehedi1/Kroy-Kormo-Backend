using Sob_Need_Server.Models.Categories;
using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Products
{
    public class ProductAttribute
    {
        public Guid AttributeId { get; set; } = Guid.NewGuid();
        
        [Required]
        [MaxLength(100)]
        public string AttributeName { get; set; } = null!;
        
        [Required]
        public Guid CategoryId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string DataType { get; set; } = "string"; // string, number, boolean, date
        
        [Required]
        public bool IsRequired { get; set; } = false;
        
        public int DisplayOrder { get; set; } = 0;
        
        [MaxLength(500)]
        public string? Options { get; set; } // JSON array for dropdown options
        
        [Required]
        public bool IsActive { get; set; } = true;
        
        [MaxLength(100)]
        public string? CreatedBy { get; set; }
        
        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual Category? Category { get; set; }
        public virtual ICollection<ProductAttributeValue> AttributeValues { get; set; } = new List<ProductAttributeValue>();
    }
}
