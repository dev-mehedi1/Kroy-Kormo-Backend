using System;
using Sob_Need_Server.Models.Authentication;

namespace Sob_Need_Server.Models.Chat
{
    public class Message
    {
        public Guid MessageId { get; set; }
        
        public Guid ConversationId { get; set; }
        public virtual Conversation Conversation { get; set; } = null!;
        
        public Guid SenderId { get; set; }
        public virtual User Sender { get; set; } = null!;
        
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
    }
}
