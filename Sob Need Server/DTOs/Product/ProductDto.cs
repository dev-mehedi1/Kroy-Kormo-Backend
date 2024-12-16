using Sob_Need_Server.DTOs.Attachment;
using Sob_Need_Server.DTOs.Location;
using Sob_Need_Server.Models.Locations;

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
        public LocationDto Location { get; set; } = null!;
        public List<ImageDto> Images { get; set; } = null!;
        public int StatusId { get; set; }
        public string Slug { get; set; } = null!;
        public bool IsJobAd { get; set; }
        public bool IsDeliveryFree { get; set; }
        public bool IsTopAd { get; set; }
        public bool IsUrgentAd { get; set; }
    }
}
