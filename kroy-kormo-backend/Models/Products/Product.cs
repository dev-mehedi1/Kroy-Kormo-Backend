using Models.Locations;
using Sob_Need_Server.Models.Authentication;
using Sob_Need_Server.Models.Categories;
using Sob_Need_Server.Models.Products;
using Sob_Need_Server.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AttachmentEntity = Models.Attachment.Attachment;

namespace Models.Products
{
    public class Product
    {
        public Guid ProductId { get; set; } = Guid.NewGuid();
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = null!;
        
        [Required]
        [MaxLength(5000)]
        public string Description { get; set; } = null!;
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Discount { get; set; }
        
        public Guid? ConditionId { get; set; }
        
        [Required]
        public Guid CategoryId { get; set; }
        
        public Guid? StatusId { get; set; }
        
        public Guid? UserId { get; set; }
        

        
        [Required]
        public bool IsVerified { get; set; } = false;
        
        [Required]
        public bool IsJobAd { get; set; } = false;
        
        [Required]
        public bool IsUrgentAd { get; set; } = false;
        
        [Required]
        public bool IsFeaturedMember { get; set; } = false;
        
        [Required]
        public bool IsDeliveryFree { get; set; } = false;
        
        [Required]
        [MaxLength(250)]
        public string Slug { get; set; } = null!;
        
        [Required]
        public bool IsTopAd { get; set; } = false;
        
        [Required]
        public bool IsSell { get; set; } = true;
        
        [Required]
        public bool IsActive { get; set; } = true;

        [Column(TypeName = "decimal(18,2)")]
        public decimal BaseScore { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal FinalScore { get; set; } = 0;

        public DateTime? LastBumpedAt { get; set; }
        
        public int ViewCount { get; set; } = 0;
        
        public int FavoriteCount { get; set; } = 0;
        
        public DateTime? ExpiresAt { get; set; }
        
        [MaxLength(100)]
        public string? CreatedBy { get; set; }
        
        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual Category? Category { get; set; }
        public virtual Condition? Condition { get; set; }
        public virtual ProductStatus? Status { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
        public virtual ICollection<ProductAttributeValue> AttributeValues { get; set; } = new List<ProductAttributeValue>();
        public virtual ICollection<ProductPromotion> Promotions { get; set; } = new List<ProductPromotion>();
        public virtual ICollection<AttachmentEntity> Attachments { get; set; } = new List<AttachmentEntity>();
        public virtual ICollection<ProductView> Views { get; set; } = new List<ProductView>();
        public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<ProductStatusHistory> StatusHistories { get; set; } = new List<ProductStatusHistory>();
    }
}
