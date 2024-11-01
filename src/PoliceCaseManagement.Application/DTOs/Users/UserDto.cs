namespace PoliceCaseManagement.Application.DTOs.Users
{
    public class UserDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string Rank { get; set; }
        public required string BadgeNumber { get; set; }
    }
}
