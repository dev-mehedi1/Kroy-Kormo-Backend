namespace Sob_Need_Server.Models.Attachment
{
    public class Attachment
    {
        public Guid AttachmentId { get; set; }
        public Guid ProductId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string? AltName { get; set; }
        public bool IsActive { get; set; } = true;
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
