namespace Sob_Need_Server.Models.Notifications
{
    public class NotificationGroup
    {
        public int NotificationGroupId { get; set; }
        public string GroupName { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
