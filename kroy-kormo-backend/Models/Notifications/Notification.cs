using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sob_Need_Server.Models.Notifications
{
    public class Notification
    {
        [Key]
        public Guid NotificationId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        // "Announcement", "Personal", "System"
        [MaxLength(50)]
        public string Type { get; set; } = "System";

        public Guid? RecipientId { get; set; }

        public bool IsRead { get; set; } = false;

        public string? ActionUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
