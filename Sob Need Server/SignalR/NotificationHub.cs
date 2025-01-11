using Microsoft.AspNetCore.SignalR;
using Sob_Need_Server.DTOs.Notification;

namespace Sob_Need_Server.SignalR
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification (NotificationDto notificationDto)
        {
            // send the message to the all connected clients
            await Clients.All.SendAsync("ReceiveNotification", notificationDto);
        }
    }
}
