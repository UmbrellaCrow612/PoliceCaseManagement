using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class ChangePasswordDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public required string NewPassword { get; set; }
    }
}
