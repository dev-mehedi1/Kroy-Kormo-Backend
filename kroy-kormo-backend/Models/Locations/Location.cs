using Models.Products;
using Sob_Need_Server.Models.Locations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Locations
{
    public class Location
    {
        public Guid LocationId { get; set; } = Guid.NewGuid();
        
        [MaxLength(500)]
        public string? AddressLine { get; set; }
        
        public Guid ProductId { get; set; }

        public Guid? CountryId { get; set; }
        
        public Guid? CityId { get; set; }
        
        public Guid? RegionId { get; set; }
        
        public Guid? StateId { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
        
        [MaxLength(100)]
        public string? CreatedBy { get; set; }
        
        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual Country? Country { get; set; }
        public virtual State? State { get; set; }
        public virtual City? City { get; set; }
        public virtual Region? Region { get; set; }
        
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;
    }
}
