using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class ValidateTOTPDto
    {
        [Required]
        public required string Code { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
