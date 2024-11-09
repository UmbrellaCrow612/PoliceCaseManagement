using System.ComponentModel.DataAnnotations;

namespace Identity.Api.DTOs
{
    /// <summary>
    /// Login in details for identity - provide either a username or email and a password.
    /// </summary>
    public class LoginDto
    {
        [EmailAddress]
        public string? Email { get; set; } = null;

        public string? UserName { get; set; } = null;

        [Required]
        public required string Password { get; set; }
    }
}
