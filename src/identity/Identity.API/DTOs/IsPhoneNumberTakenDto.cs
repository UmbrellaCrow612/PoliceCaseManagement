using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class IsPhoneNumberTakenDto
    {
        [Required]
        public required string PhoneNumber { get; set; }
    }
}
