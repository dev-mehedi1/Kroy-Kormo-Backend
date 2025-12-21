namespace Sob_Need_Server.DTOs.Authentication
{
    public class UpdateUserDto
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Bio { get; set; }
        public string? Address { get; set; }
    }
}
