using System.ComponentModel.DataAnnotations;

namespace PoliceCaseManagement.Application.DTOs.Users
{
    public class CreateUserDto
    {
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Password { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Rank { get; set; }
        [Required]
        public required string BadgeNumber { get; set; }
    }
}
