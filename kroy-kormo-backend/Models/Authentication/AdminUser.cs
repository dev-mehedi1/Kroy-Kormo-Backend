using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.Models.Authentication
{
    public class AdminUser
    {
        [Key]
        public Guid AdminId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string Password { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = null!;

        [MaxLength(20)]
        public string? Phone { get; set; }

        public string Role { get; set; } = "Admin"; // Admin, SuperAdmin

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLoginAt { get; set; }
    }
}
