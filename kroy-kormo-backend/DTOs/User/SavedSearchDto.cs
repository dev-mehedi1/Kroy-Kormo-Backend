namespace Sob_Need_Server.DTOs.User
{
    public class SavedSearchDto
    {
        public Guid SearchId { get; set; }
        public Guid UserId { get; set; }
        public string SearchName { get; set; } = null!;
        public string FilterJson { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
