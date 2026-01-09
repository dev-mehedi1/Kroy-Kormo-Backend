namespace Sob_Need_Server.DTOs.Jobs
{
    public class SkillDto
    {
        public Guid SkillId { get; set; }
        public string SkillName { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
