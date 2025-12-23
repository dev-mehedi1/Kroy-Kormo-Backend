namespace Sob_Need_Server.DTOs.Authentication
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Bio { get; set; }
        public string? Address { get; set; }
        public Guid? UserTypeId { get; set; }
        public string? UserTypeName { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Token { get; set; } // JWT token for authentication
    }
}
