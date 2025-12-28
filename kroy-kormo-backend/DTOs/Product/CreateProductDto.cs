using Sob_Need_Server.DTOs.Location;

namespace Sob_Need_Server.DTOs.Product
{
    public class CreateProductDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public Guid ConditionId { get; set; }
        public Guid CategoryId { get; set; }
        public List<LocationDto>? Locations { get; set; }
        public bool IsJobAd { get; set; } = false;
        public bool IsDeliveryFree { get; set; } = false;
        public bool IsSell { get; set; } = true;
        public List<ProductAttributeValueDto>? Attributes { get; set; }
        public List<string>? ImageUrls { get; set; }
    }
}
