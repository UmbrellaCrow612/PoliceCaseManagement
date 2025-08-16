using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class VerifyEmailDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Code { get; set; }
    }
}
