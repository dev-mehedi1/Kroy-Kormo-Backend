using Models.Products;
using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Products
{
    public class Condition
    {
        [Key]
        public Guid ConditionId { get; set; } = Guid.NewGuid();
        
        [Required]
        [MaxLength(50)]
        public string ConditionName { get; set; } = null!;
        
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
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
