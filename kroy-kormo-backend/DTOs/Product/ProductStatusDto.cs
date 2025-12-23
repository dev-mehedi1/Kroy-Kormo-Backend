namespace Sob_Need_Server.DTOs.Product
{
    public class ProductStatusDto
    {
        public Guid StatusId { get; set; }
        public string StatusName { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Description { get; set; }
    }
}
