using Sob_Need_Server.Models.Products;
using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Categories
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public Guid? ParentCategoryId { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;

        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Product> Products { get; set; } = [];

    }
}
