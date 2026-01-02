using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sob_Need_Server.DTOs.Chat;
using Sob_Need_Server.DTOs.Common;

namespace Sob_Need_Server.Interface.Chat
{
    public interface IChatService
    {
        Task<ResponseDto<IEnumerable<ConversationDto>>> GetConversationsAsync(Guid userId);
        Task<ResponseDto<IEnumerable<MessageDto>>> GetMessagesAsync(Guid conversationId, Guid userId);
        Task<ResponseDto<MessageDto>> SendMessageAsync(Guid senderId, Guid receiverId, string content);
        Task<ResponseDto<bool>> MarkAsReadAsync(Guid conversationId, Guid userId);
        Task<ResponseDto<int>> GetUnreadCountAsync(Guid userId);
    }
}
