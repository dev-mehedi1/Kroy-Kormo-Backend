using System;

namespace Sob_Need_Server.DTOs.Chat
{
    public class ConversationDto
    {
        public Guid ConversationId { get; set; }
        public Guid ParticipantId { get; set; }
        public string ParticipantName { get; set; } = null!;
        public string? ParticipantImage { get; set; }
        public string? LastMessage { get; set; }
        public DateTime? LastMessageAt { get; set; }
        public int UnreadCount { get; set; }
    }
}
