using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Sob_Need_Server.Utils.Helpers;

public class JwtHelper
{
    public static string GetUserIdFromToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return null!;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token.Replace("Bearer ", ""));

        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        return userIdClaim!;
    }
}
