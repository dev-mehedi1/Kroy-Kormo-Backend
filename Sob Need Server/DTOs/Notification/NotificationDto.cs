namespace Sob_Need_Server.DTOs.Notification
{
    public class NotificationDto
    {
        public int NotificationId { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
