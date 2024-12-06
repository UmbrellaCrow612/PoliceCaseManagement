using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class PhoneConfirmationDto
    {
        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }

        [Required]
        public required string Code { get; set; }
    }
}
