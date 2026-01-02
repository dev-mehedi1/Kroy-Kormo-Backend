using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.Chat;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.Interface.Chat;
using Sob_Need_Server.Utils;
using Sob_Need_Server.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sob_Need_Server.Controllers.Chat
{
    [Route(Constants.BasePath + "/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController(IChatService chatService, IHttpContextAccessor httpContextAccessor) : ControllerBase
    {
        private readonly IChatService _chatService = chatService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        [HttpGet("conversations")]
        public async Task<ActionResult<ResponseDto<IEnumerable<ConversationDto>>>> GetConversations()
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var res = await _chatService.GetConversationsAsync(userId.Value);
            return Ok(res);
        }

        [HttpGet("messages/{conversationId}")]
        public async Task<ActionResult<ResponseDto<IEnumerable<MessageDto>>>> GetMessages(Guid conversationId)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var res = await _chatService.GetMessagesAsync(conversationId, userId.Value);
            return Ok(res);
        }

        [HttpPost("send")]
        public async Task<ActionResult<ResponseDto<MessageDto>>> SendMessage([FromBody] SendMessageRequest request)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var res = await _chatService.SendMessageAsync(userId.Value, request.ReceiverId, request.Content);
            return Ok(res);
        }

        [HttpPost("read/{conversationId}")]
        public async Task<ActionResult<ResponseDto<bool>>> MarkAsRead(Guid conversationId)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var res = await _chatService.MarkAsReadAsync(conversationId, userId.Value);
            return Ok(res);
        }

        private Guid? GetCurrentUserId()
        {
            var userIdStr = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdStr)) return null;
            return Guid.Parse(userIdStr);
        }
    }

    public class SendMessageRequest
    {
        public Guid ReceiverId { get; set; }
        public string Content { get; set; } = null!;
    }
}
