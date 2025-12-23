using System;

namespace Sob_Need_Server.DTOs.Chat
{
    public class ConversationDto
    {
        public Guid ConversationId { get; set; }
        public Guid OtherUserId { get; set; }
        public string OtherUserName { get; set; } = null!;
        public string? OtherUserImageUrl { get; set; }
        public string? LastMessage { get; set; }
        public DateTime? LastMessageAt { get; set; }
        public int UnreadCount { get; set; }
    }
}
