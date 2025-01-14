using Sob_Need_Server.DTOs.Notification;
using Sob_Need_Server.DTOs.Common;

namespace Sob_Need_Server.Interface.Notification;

public interface INotificationService : IGenericService<NotificationDto, int>
{
    Task<ResponseDto<IEnumerable<NotificationDto>>> GetNotificationsAsync(FilterDto filter);
}
