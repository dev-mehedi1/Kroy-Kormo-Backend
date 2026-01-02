using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Sob_Need_Server.Utils.Helpers;

public class JwtHelper
{
    public static string GetUserIdFromToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return null!;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(Regex.Replace(token, "Bearer ", "", RegexOptions.IgnoreCase));

        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        return userIdClaim!;
    }
}
