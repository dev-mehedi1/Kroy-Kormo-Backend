using System;
using System.Collections.Generic;
using Sob_Need_Server.Models.Authentication;

namespace Sob_Need_Server.Models.Chat
{
    public class Conversation
    {
        public Guid ConversationId { get; set; }
        
        public Guid User1Id { get; set; }
        public virtual User User1 { get; set; } = null!;
        
        public Guid User2Id { get; set; }
        public virtual User User2 { get; set; } = null!;
        
        public string? LastMessage { get; set; }
        public DateTime? LastMessageAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
