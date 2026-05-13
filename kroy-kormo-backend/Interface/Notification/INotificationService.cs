using Sob_Need_Server.DTOs.Notification;
using Sob_Need_Server.DTOs.Common;

namespace Sob_Need_Server.Interface.Notification;

public interface INotificationService : IGenericService<NotificationDto, Guid>
{
    Task<ResponseDto<IEnumerable<NotificationDto>>> GetNotificationsAsync(FilterDto filter);
    Task<ResponseDto<IEnumerable<NotificationDto>>> GetMyNotificationsAsync(Guid userId);
    Task<ResponseDto<bool>> MarkAsReadAsync(Guid notificationId, Guid userId);
    Task<ResponseDto<NotificationDto>> SendAnnouncementAsync(NotificationDto announcementDto);
}
