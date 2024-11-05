using System.ComponentModel.DataAnnotations;

namespace PoliceCaseManagement.Application.DTOs.Auth
{
    public class CreateLoginRequestDto
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
