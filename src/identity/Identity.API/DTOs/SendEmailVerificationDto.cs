using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class SendEmailVerificationDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
