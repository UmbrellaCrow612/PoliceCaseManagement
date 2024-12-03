using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class ReSendPhoneConfirmationDto
    {
        [EmailAddress]
        [Required]
        public required string Email { get; set; }
    }
}
