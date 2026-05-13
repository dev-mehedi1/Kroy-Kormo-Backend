using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Notification;
using Sob_Need_Server.Interface.Notification;
using Sob_Need_Server.Models.Notifications;
using Sob_Need_Server.SignalR;

namespace Sob_Need_Server.Services.Notifications
{
    public class NotificationService(AppDbContext context, IMapper mapper, IHubContext<NotificationHub> hubContext) : INotificationService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IHubContext<NotificationHub> _hubContext = hubContext;

        public async Task<ResponseDto<NotificationDto>> CreateAsync(NotificationDto entity)
        {
            try
            {
                var notificationEntity = _mapper.Map<Notification>(entity);
                notificationEntity.CreatedAt = DateTime.UtcNow;

                await _context.Notifications.AddAsync(notificationEntity);
                await _context.SaveChangesAsync();

                var notificationDto = _mapper.Map<NotificationDto>(notificationEntity);

                // If it's a personal notification, send to specific user
                if (notificationEntity.RecipientId.HasValue)
                {
                    var userIdStr = notificationEntity.RecipientId.Value.ToString();
                     await _hubContext.Clients.User(userIdStr).SendAsync("ReceiveNotification", notificationDto);
                }

                // If it's an announcement, we might want to broadcast or handle differently
                // For now, let's assume 'System' or 'Announcement' might go to everyone or a group
                if (notificationEntity.Type == "Announcement")
                {
                     await _hubContext.Clients.All.SendAsync("ReceiveNotification", notificationDto);
                }

                return new ResponseDto<NotificationDto>(true, $"Notification Created Successful", notificationDto);
            }
            catch (Exception ex)
            {
                return new ResponseDto<NotificationDto>(false, $"{ex.Message}", null!);
            }
        }

        public async Task<ResponseDto<IEnumerable<NotificationDto>>> GetMyNotificationsAsync(Guid userId)
        {
            try
            {
                // Get Personal notifications OR Announcements
                // Assuming Announcements have RecipientId == null and Type == "Announcement"
                var notifications = await _context.Notifications
                    .Where(n => (n.RecipientId == userId) || (n.RecipientId == null && n.Type == "Announcement"))
                    .OrderByDescending(n => n.CreatedAt)
                    .ToListAsync();

                var notificationDtos = _mapper.Map<IEnumerable<NotificationDto>>(notifications);
                return new ResponseDto<IEnumerable<NotificationDto>>(true, "Notifications fetched successfully", notificationDtos);
            }
            catch(Exception ex)
            {
                return new ResponseDto<IEnumerable<NotificationDto>>(false, ex.Message, null!);
            }
        }

        public async Task<ResponseDto<bool>> MarkAsReadAsync(Guid notificationId, Guid userId)
        {
             try
            {
                var notification = await _context.Notifications.FindAsync(notificationId);
                if (notification == null) return new ResponseDto<bool>(false, "Notification not found", false);

                // Only allow recipient to mark as read
                // For announcements (global), this logic might need a separate 'UserReadNotification' table to track distinct reads.
                // For now, let's implement simple IsRead toggle for Personal ones.
                
                if (notification.RecipientId == userId)
                {
                    notification.IsRead = true;
                    await _context.SaveChangesAsync();
                    return new ResponseDto<bool>(true, "Marked as read", true);
                }
                
                return new ResponseDto<bool>(false, "Unauthorized or global notification", false);
            }
            catch (Exception ex)
            {
                  return new ResponseDto<bool>(false, ex.Message, false);
            }
        }

        public async Task<ResponseDto<NotificationDto>> SendAnnouncementAsync(NotificationDto announcementDto)
        {
            announcementDto.RecipientId = null;
            announcementDto.Type = "Announcement";
            return await CreateAsync(announcementDto);
        }

        // Methods below are placeholders or pending implementation if needed
        public Task<ResponseDto<NotificationDto>> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<NotificationDto>> GetByIdAsync(Guid id)
        {
             throw new NotImplementedException();
        }

        public Task<ResponseDto<IEnumerable<NotificationDto>>> GetNotificationsAsync(FilterDto filter)
        {
            // Implementation for generic filter if needed
            throw new NotImplementedException();
        }

        public Task<ResponseDto<NotificationDto>> UpdateAsync(Guid id, NotificationDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
