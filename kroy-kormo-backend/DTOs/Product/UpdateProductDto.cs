namespace Sob_Need_Server.DTOs.Product
{
    public class UpdateProductDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public Guid? ConditionId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? LocationId { get; set; }
        public bool? IsJobAd { get; set; }
        public bool? IsDeliveryFree { get; set; }
        public bool? IsSell { get; set; }
        public List<ProductAttributeValueDto>? Attributes { get; set; }
        public List<string>? ImageUrls { get; set; }
    }
}
