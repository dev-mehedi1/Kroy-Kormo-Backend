using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Notification;
using Sob_Need_Server.Interface.Notification;
using Sob_Need_Server.Models.Notification;
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
                // Map DTO to Entity
                var notificationEntity = _mapper.Map<Notification>(entity);
                notificationEntity.CreatedAt = DateTime.UtcNow;

                await _context.Notifications.AddAsync(notificationEntity);
                await _context.SaveChangesAsync();

                var notificationDto = _mapper.Map<NotificationDto>(notificationEntity);
                // for testing token user 
                var userId = "65c8bed2-dbcb-4044-a6ef-050935f542aa";
                var groupName = "sample";
                await _hubContext.Clients.User(userId)
                   .SendAsync("ReceiveNotification", notificationDto);

                // Send notification to a group if groupName is provided
                if (!string.IsNullOrEmpty(groupName))
                {
                    await _hubContext.Clients.Group(groupName)
                        .SendAsync("ReceiveGroupNotification", notificationDto);
                }


                return new ResponseDto<NotificationDto>(true, $"Notification Created Successful", notificationDto);
            }
            catch (Exception ex)
            {
               return new ResponseDto<NotificationDto>(false, $"{ex.Message}", null!);
            }
        }

        public Task<ResponseDto<NotificationDto>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<NotificationDto>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<IEnumerable<NotificationDto>>> GetNotificationsAsync(FilterDto filter)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<NotificationDto>> UpdateAsync(int id, NotificationDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
