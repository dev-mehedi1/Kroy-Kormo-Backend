namespace Sob_Need_Server.Models.Notifications
{
    public class GroupMembers
    {
        public int GroupMembersId { get; set; }
        public int GroupId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
