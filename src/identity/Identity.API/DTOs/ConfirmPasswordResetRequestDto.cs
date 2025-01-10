using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class ConfirmPasswordResetRequestDto
    {
        [Required]
        public required string NewPassword { get; set; }
    }
}
