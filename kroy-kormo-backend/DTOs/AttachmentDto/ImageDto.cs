namespace Sob_Need_Server.DTOs.Attachment
{
    public class ImageDto
    {
        public Guid AttachmentId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string? AltName { get; set; }

    }
}
