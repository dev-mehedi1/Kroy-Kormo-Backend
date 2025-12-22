namespace Sob_Need_Server.Models.Notifications
{
    public class Notification
    {
        public Guid NotificationId { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty ;
        public DateTime CreatedAt { get; set; }
    }
}
