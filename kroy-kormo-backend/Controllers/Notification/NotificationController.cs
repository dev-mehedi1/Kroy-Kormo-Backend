using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Notification;
using Sob_Need_Server.Interface.Notification;

namespace Sob_Need_Server.Controllers.Notification;

[Route("api/[controller]")]
[ApiController]
public class NotificationController(INotificationService notificationService) : ControllerBase
{
    private readonly INotificationService _notificationService = notificationService;

    [HttpPost]
    public async Task<IActionResult> CreateNotification([FromBody] NotificationDto notificationDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResponseDto<NotificationDto>(false, "Invalid request data.", null!));
        }

        var response = await _notificationService.CreateAsync(notificationDto);

        if (response.IsSuccess)
        {
            return  Ok(response);
        }

        return BadRequest(response);
    }
}
