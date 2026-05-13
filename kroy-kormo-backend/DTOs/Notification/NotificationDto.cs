namespace Sob_Need_Server.DTOs.Notification
{
    public class NotificationDto
    {
        public Guid NotificationId { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string Type { get; set; } = "System";
        public Guid? RecipientId { get; set; }
        public bool IsRead { get; set; }
        public string? ActionUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
