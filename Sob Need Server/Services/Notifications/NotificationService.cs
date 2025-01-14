using AutoMapper;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Notification;
using Sob_Need_Server.Interface.Notification;
using Sob_Need_Server.Models.Notification;

namespace Sob_Need_Server.Services.Notifications
{
    public class NotificationService(AppDbContext context, IMapper mapper) : INotificationService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;
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
