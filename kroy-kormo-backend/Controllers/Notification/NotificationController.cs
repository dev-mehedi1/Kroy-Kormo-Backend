using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Notification;
using Sob_Need_Server.Interface.Notification;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Sob_Need_Server.Controllers.Notification;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class NotificationController(INotificationService notificationService) : ControllerBase
{
    private readonly INotificationService _notificationService = notificationService;

    [HttpPost]
    public async Task<IActionResult> CreateNotification([FromBody] NotificationDto notificationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResponseDto<NotificationDto>(false, "Invalid request data.", null!));

        var response = await _notificationService.CreateAsync(notificationDto);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpGet("my-notifications")]
    public async Task<IActionResult> GetMyNotifications()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            return Unauthorized(new ResponseDto<object>(false, "User ID not found in token", null!));

        var response = await _notificationService.GetMyNotificationsAsync(userId);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            return Unauthorized(new ResponseDto<object>(false, "User ID not found in token", null!));

        var response = await _notificationService.MarkAsReadAsync(id, userId);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpPost("announce")]
    [Authorize(Roles = "SuperAdmin,Admin,Owner")]
    public async Task<IActionResult> SendAnnouncement([FromBody] NotificationDto notificationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResponseDto<NotificationDto>(false, "Invalid request data.", null!));

        var response = await _notificationService.SendAnnouncementAsync(notificationDto);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }
}
