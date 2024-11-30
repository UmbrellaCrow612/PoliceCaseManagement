using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class ResendConfirmationEmailDto
    {
        [EmailAddress]
        [Required]
        public required string Email { get; set; }
    }
}
