using Microsoft.AspNetCore.SignalR;

namespace Sob_Need_Server.SignalR
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            // Extract the "UserId" claim from the JWT
            return connection.User?.FindFirst("UserId")?.Value;
        }
    }
}
