using DTOs.Product;
using Sob_Need_Server.DTOs.Attachment;
using Sob_Need_Server.DTOs.Authentication;
using Sob_Need_Server.DTOs.Location;

namespace Sob_Need_Server.DTOs.Product
{
    public class ProductDto
    {
        public Guid ProductId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        
        // Condition
        public Guid ConditionId { get; set; }
        public string? ConditionName { get; set; }
        
        // Category
        public Guid CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? ParentCategoryName { get; set; }
        
        // Status
        public Guid StatusId { get; set; }
        public string? StatusName { get; set; }
        
        // User/Seller info
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? SellerName { get; set; }
        public string? SellerPhone { get; set; }
        public bool IsVerifiedSeller { get; set; }
        public string? UserTypeName { get; set; }
        
        // Location
        public List<LocationDto> Locations { get; set; } = new List<LocationDto>();
        
        // Images
        public List<ImageDto> Images { get; set; } = new List<ImageDto>();
        
        // Attributes
        public List<ProductAttributeValueDto> Attributes { get; set; } = new List<ProductAttributeValueDto>();
        
        // Flags
        public string Slug { get; set; } = null!;
        public bool IsJobAd { get; set; }
        public bool IsDeliveryFree { get; set; }
        public bool IsTopAd { get; set; }
        public bool IsUrgentAd { get; set; }
        public bool IsFeaturedMember { get; set; }
        public bool IsSell { get; set; }
        public bool IsVerified { get; set; }
        
        // Statistics
        public int ViewCount { get; set; }
        public int FavoriteCount { get; set; }
        
        // Dates
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }

        // Promotions
        public List<PromotionDto> ActivePromotions { get; set; } = new List<PromotionDto>();

        // Status History (Timeline)
        public List<ProductStatusHistoryDto> StatusHistory { get; set; } = new List<ProductStatusHistoryDto>();
    }
}

