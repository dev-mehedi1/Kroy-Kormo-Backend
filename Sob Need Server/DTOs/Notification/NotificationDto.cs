namespace Sob_Need_Server.DTOs.Notification
{
    public class NotificationDto
    {
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime Time { get; set; }
    }
}
