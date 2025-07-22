using Microsoft.AspNetCore.SignalR;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Notification;
using Sob_Need_Server.Models.Notification;
using Sob_Need_Server.Models.Notifications;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Sob_Need_Server.SignalR
{
    public class NotificationHub(AppDbContext context) : Hub
    {
        private readonly AppDbContext _context = context;

        private static readonly ConcurrentDictionary<string, string> UserConnections = new();

        //public override Task OnConnectedAsync()
        //{
        //    var userId = Context.UserIdentifier; // Assuming you use UserIdentifier
        //    if (!string.IsNullOrEmpty(userId))
        //    {
        //        Console.WriteLine("User Connected:" + userId);
        //        UserConnections[userId] = Context.ConnectionId;
        //    }

        //    return base.OnConnectedAsync();
        //}


        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier; // Assuming UserIdentifier contains the user's ID

            if (!string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("User Connected: " + userId);

                // Map the user's connection ID
                UserConnections[userId] = Context.ConnectionId;

                // Retrieve the groups this user belongs to
                var userGroups = _context.GroupMembers
                    .Join(_context.NotificationGroup,
                          gm => gm.GroupId,
                          ng => ng.NotificationGroupId,
                          (gm, ng) => new { gm.UserId, ng.GroupName })
                    .Where(x => x.UserId == Guid.Parse(userId))
                    .Select(x => x.GroupName)
                    .ToList();

                // Add the user to each of their groups
                foreach (var groupName in userGroups)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                    Console.WriteLine($"User {userId} added to group {groupName}");
                }
            }

            await base.OnConnectedAsync();
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

        // Create a group
        public async Task CreateGroup(string groupName)
        {
            var group = new NotificationGroup
            {
                GroupName = groupName,
                CreatedAt = DateTime.UtcNow
            };

            _context.NotificationGroup.Add(group);
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("GroupCreated", group);
        }

        // Dynamically join a group using groupName and userId
        public async Task JoinGroup(string groupName, string userId)
        {
            var group = _context.NotificationGroup.FirstOrDefault(g => g.GroupName == groupName);
            if (group == null)
            {
                throw new Exception("Group does not exist.");
            }

            var groupMember = new GroupMembers
            {
                GroupId = group.NotificationGroupId,
                UserId = Guid.Parse(userId),
                CreatedAt = DateTime.UtcNow
            };

            _context.GroupMembers.Add(groupMember);
            await _context.SaveChangesAsync();

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("UserJoinedGroup", userId, groupName);
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
        // Send a message to a group
        public async Task SendMessageToGroup(string groupName, string userId, string message)
        {
            var group = _context.NotificationGroup.FirstOrDefault(g => g.GroupName == groupName);
            if (group == null)
            {
                throw new Exception("Group does not exist.");
            }

            var groupMessage = new Notification
            {
                Message = message,
                Title = groupName,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(groupMessage);
            await _context.SaveChangesAsync();

            await Clients.Group(groupName).SendAsync("ReceiveNotification", userId, message, groupName);
        }
    }
}
