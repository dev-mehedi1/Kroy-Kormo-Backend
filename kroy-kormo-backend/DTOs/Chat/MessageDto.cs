using System;

namespace Sob_Need_Server.DTOs.Chat
{
    public class MessageDto
    {
        public Guid MessageId { get; set; }
        public Guid ConversationId { get; set; }
        public Guid SenderId { get; set; }
        public string SenderName { get; set; } = null!;
        public string? SenderImageUrl { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        
        public Guid? ProductId { get; set; }
        public string? ProductTitle { get; set; }
        public string? ProductImageUrl { get; set; }
    }
}
