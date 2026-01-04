namespace Sob_Need_Server.Models.Notifications
{
    public class GroupMembers
    {
        public Guid GroupMembersId { get; set; } = Guid.NewGuid();
        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
