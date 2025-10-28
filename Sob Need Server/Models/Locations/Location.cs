using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Locations
{
    public class Location
    {
        public Guid LocationId { get; set; }
        public string? AddressLine { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? CityId { get; set; }
        public Guid? RegionId { get; set; }
        public Guid? StateId { get; set; }
        public Guid ProductId { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
        public string? CreatedBy { get; set; }
        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
