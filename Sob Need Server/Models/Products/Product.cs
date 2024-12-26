namespace Sob_Need_Server.Models.Products
{
    public class Product
    {
        public int ProductId  { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Price { get; set; } = null!;
        public string? Discount { get; set; }
        public int ConditionId { get; set; }
        public int CategoryId { get; set; }
        public int StatusId { get; set; }
        public bool IsVerified { get; set; }
        public bool IsJobAd { get; set; }
        public bool IsUrgentAd { get; set; }
        public bool IsFeaturedMember { get; set; }
        public bool IsDeliveryFree { get; set; }
        public string Slug { get; set; } = null!;
        public bool IsTopAd { get; set; }
        public bool IsSell { get; set; }
        public bool IsActive { get; set; } = true;
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
