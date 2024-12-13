using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Locations
{
    public class Location
    {
        public int LocationId { get; set; }
        public string? AddressLine { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public int? RegionId { get; set; }
        public int? StateId { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
        public string? CreatedBy { get; set; }
        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
