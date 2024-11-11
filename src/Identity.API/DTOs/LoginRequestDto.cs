using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class LoginRequestDto
    {
        public string? UserName { get; set; } = null;

        [EmailAddress]
        public string? Email { get; set; } = null;

        [Required]
        public required string Password { get; set; }
    }
}
