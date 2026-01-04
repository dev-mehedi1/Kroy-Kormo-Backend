using Microsoft.IdentityModel.Tokens;
using Sob_Need_Server.Models.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Sob_Need_Server.Utils.Helpers
{
    public class TokenHelper(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        // Method to generate JWT token
        public string GenerateJwtToken(User user)
        {
            // Get secret key from configuration
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Set JWT claims
            var claims = new[]
            {
                new Claim("UserName", user.UserName), 
                new Claim("TokenId", Guid.NewGuid().ToString()),
                new Claim("UserId", user.UserId.ToString()),
                new Claim("FullName", user.FullName),
                new Claim("Email", user.Email)
            };

            // Generate token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
