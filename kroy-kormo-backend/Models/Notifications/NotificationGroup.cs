namespace Sob_Need_Server.Models.Notifications
{
    public class NotificationGroup
    {
        public Guid NotificationGroupId { get; set; } = Guid.NewGuid();
        public string GroupName { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
