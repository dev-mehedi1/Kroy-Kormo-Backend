using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Locations
{
    public class State
    {
        public Guid StateId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        public string StateName { get; set; }

        public Guid CountryId { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
