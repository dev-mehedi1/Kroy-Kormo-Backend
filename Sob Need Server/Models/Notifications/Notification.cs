namespace Sob_Need_Server.Models.Notification
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty ;
        public DateTime CreatedAt { get; set; }
    }
}
