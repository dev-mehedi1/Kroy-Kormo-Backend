namespace Sob_Need_Server.DTOs.Authentication
{
    public class UserRegistrationDto
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public Guid? UserTypeId { get; set; }
    }
}
