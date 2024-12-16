namespace Sob_Need_Server.DTOs.Product
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Price { get; set; } = null!;
        public string? Discount { get; set; }
        public int ConditionId { get; set; }
        public int CategoryId { get; set; }
        public int LocationId { get; set; }
        public int StatusId { get; set; }
        public int AttachmentId { get; set; }
        public string Slug { get; set; } = null!;
        public bool IsJobAd { get; set; }
        public bool IsDeliveryFree { get; set; }
        public bool IsTopAd { get; set; }
        public bool IsUrgentAd { get; set; }
    }
}
