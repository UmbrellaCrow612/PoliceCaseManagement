using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class SendPhoneConfirmationDto
    {
        [Phone]
        [Required]
        public required string PhoneNumber { get; set; }
    }
}
