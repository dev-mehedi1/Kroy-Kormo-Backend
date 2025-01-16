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
                Console.WriteLine("User Connected:" + userId);
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

        // Dynamically join a group using groupName and userId
        public async Task JoinGroup(string groupName, string userId)
        {
            if (UserConnections.TryGetValue(userId, out var connectionId))
            {
                await Groups.AddToGroupAsync(connectionId, groupName);
                Console.WriteLine($"User {userId} (Connection: {connectionId}) joined group {groupName}");
            }
            else
            {
                Console.WriteLine($"User {userId} is not connected. Unable to join group {groupName}.");
            }
        }

        // Leave a group
        public async Task LeaveGroup(string groupName, string userId)
        {
            if (UserConnections.TryGetValue(userId, out var connectionId))
            {
                await Groups.RemoveFromGroupAsync(connectionId, groupName);
                Console.WriteLine($"User {userId} (Connection: {connectionId}) left group {groupName}");
            }
            else
            {
                Console.WriteLine($"User {userId} is not connected. Unable to leave group {groupName}.");
            }
        }

        // Send a message to a specific group
        public async Task SendMessageToGroup(string groupName, NotificationDto notificationDto)
        {
            await Clients.Group(groupName).SendAsync("ReceiveNotification", notificationDto);
        }
    }
}
