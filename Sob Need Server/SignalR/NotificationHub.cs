using Microsoft.AspNetCore.SignalR;
using Sob_Need_Server.DTOs.Notification;
using System.Collections.Concurrent;

namespace Sob_Need_Server.SignalR
{
    public class NotificationHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> UserConnections = new();

        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier; // Assuming you use UserIdentifier
            if (!string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("User Connected" + userId);
                UserConnections[userId] = Context.ConnectionId;
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier; // Assuming you use UserIdentifier
            if (!string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("User Disconnected" + userId);
                UserConnections.TryRemove(userId, out _);
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotificationToUser(string userId, NotificationDto notificationDto)
        {
            if (UserConnections.TryGetValue(userId, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveNotification", notificationDto);
            }
        }
    }
}
