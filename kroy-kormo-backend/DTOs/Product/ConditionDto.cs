namespace Sob_Need_Server.DTOs.Product
{
    public class ConditionDto
    {
        public Guid ConditionId { get; set; }
        public string ConditionName { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Description { get; set; }
    }
}
