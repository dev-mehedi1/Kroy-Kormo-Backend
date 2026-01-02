using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Chat;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.Interface.Chat;
using Sob_Need_Server.Models.Chat;
using Sob_Need_Server.SignalR;
using AutoMapper;

namespace Sob_Need_Server.Services.Chat
{
    public class ChatService(AppDbContext context, IHubContext<NotificationHub> hubContext, IMapper mapper) : IChatService
    {
        private readonly AppDbContext _context = context;
        private readonly IHubContext<NotificationHub> _hubContext = hubContext;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseDto<IEnumerable<ConversationDto>>> GetConversationsAsync(Guid userId)
        {
            var conversations = await _context.Conversations
                .Include(c => c.User1)
                .Include(c => c.User2)
                .Include(c => c.Messages)
                .Where(c => c.User1Id == userId || c.User2Id == userId)
                .OrderByDescending(c => c.LastMessageAt)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<ConversationDto>>(conversations, opt => opt.Items["UserId"] = userId);

            return new ResponseDto<IEnumerable<ConversationDto>>(true, "Conversations retrieved", dtos);
        }

        public async Task<ResponseDto<IEnumerable<MessageDto>>> GetMessagesAsync(Guid conversationId, Guid userId)
        {
            var conversationExists = await _context.Conversations.AnyAsync(c => c.ConversationId == conversationId && (c.User1Id == userId || c.User2Id == userId));
            if (!conversationExists)
                return new ResponseDto<IEnumerable<MessageDto>>(false, "Conversation not found or access denied", null!);

            var messages = await _context.Messages
                .Include(m => m.Sender)
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<MessageDto>>(messages);

            return new ResponseDto<IEnumerable<MessageDto>>(true, "Messages retrieved", dtos);
        }

        public async Task<ResponseDto<MessageDto>> SendMessageAsync(Guid senderId, Guid receiverId, string content)
        {
            // Validate receiver exists
            var receiverExists = await _context.Users.AnyAsync(u => u.UserId == receiverId);
            if (!receiverExists)
                return new ResponseDto<MessageDto>(false, "Receiver not found", null!);

            // Ensure unique conversation between two users
            var user1Id = senderId.CompareTo(receiverId) < 0 ? senderId : receiverId;
            var user2Id = senderId.CompareTo(receiverId) < 0 ? receiverId : senderId;

            var conversation = await _context.Conversations
                .FirstOrDefaultAsync(c => c.User1Id == user1Id && c.User2Id == user2Id);

            if (conversation == null)
            {
                conversation = new Conversation
                {
                    User1Id = user1Id,
                    User2Id = user2Id,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Conversations.Add(conversation);
            }

            var message = new Message
            {
                ConversationId = conversation.ConversationId,
                SenderId = senderId,
                Content = content,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            conversation.LastMessage = content;
            conversation.LastMessageAt = message.CreatedAt;

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            // Reload message to include sender for mapping
            var savedMessage = await _context.Messages
                .Include(m => m.Sender)
                .FirstOrDefaultAsync(m => m.MessageId == message.MessageId);

            var messageDto = _mapper.Map<MessageDto>(savedMessage);

            // Broadcast to both receiver and sender (for other active sessions) via SignalR
            await _hubContext.Clients.Users(senderId.ToString(), receiverId.ToString()).SendAsync("ReceiveChatMessage", messageDto);

            return new ResponseDto<MessageDto>(true, "Message sent", messageDto);
        }

        public async Task<ResponseDto<bool>> MarkAsReadAsync(Guid conversationId, Guid userId)
        {
            var messages = await _context.Messages
                .Where(m => m.ConversationId == conversationId && m.SenderId != userId && !m.IsRead)
                .ToListAsync();

            foreach (var m in messages)
            {
                m.IsRead = true;
            }

            await _context.SaveChangesAsync();
            return new ResponseDto<bool>(true, "Messages marked as read", true);
        }

        public async Task<ResponseDto<int>> GetUnreadCountAsync(Guid userId)
        {
            var count = await _context.Messages
                .CountAsync(m => m.Conversation.Messages.Any(msg => msg.Conversation.User1Id == userId || msg.Conversation.User2Id == userId) // Access denied check implicitly via conversation
                                 && m.SenderId != userId 
                                 && !m.IsRead 
                                 && (m.Conversation.User1Id == userId || m.Conversation.User2Id == userId));

            return new ResponseDto<int>(true, "Unread count retrieved", count);
        }
    }
}
