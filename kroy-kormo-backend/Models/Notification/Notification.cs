using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sob_Need_Server.Models.Notification
{
    public class Notification
    {
        [Key]
        public Guid NotificationId { get; set; } = Guid.NewGuid();

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Body { get; set; } = null!;

        // "Announcement", "Personal", "System"
        [Required]
        public string Type { get; set; } = "System"; 

        // If Personal, this is the userId. If Announcement, could be null or checking specific logic.
        public Guid? RecipientId { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Optional: Link to a resource (e.g., /jobs/123)
        public string? ActionUrl { get; set; }
    }
}
