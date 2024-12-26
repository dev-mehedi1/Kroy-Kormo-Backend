using Microsoft.AspNetCore.Identity;

namespace Sob_Need_Server.Utils.Helpers
{
    public class PasswordHelper
    {
        private static readonly PasswordHasher<object> PasswordHasher = new();

        /// <summary>
        /// Generates a hashed password.
        /// </summary>
        /// <param name="password">The plain text password to hash.</param>
        /// <returns>The hashed password.</returns>
        public static string GenerateHashedPassword(string password)
        {
            return PasswordHasher.HashPassword(null!, password);

        }


        /// <summary>
        /// Verifies a hashed password with a provided plain text password.
        /// </summary>
        /// <param name="hashedPassword">The stored hashed password.</param>
        /// <param name="plainPassword">The plain text password to verify.</param>
        /// <returns>True if the password matches; otherwise, false.</returns>
        /// 
       
        public static bool VerifyPassword(string hashedPassword, string plainPassword)
        {
           var result = PasswordHasher.VerifyHashedPassword(null!, hashedPassword, plainPassword);

           return result == PasswordVerificationResult.Success;
        }



    }
}
