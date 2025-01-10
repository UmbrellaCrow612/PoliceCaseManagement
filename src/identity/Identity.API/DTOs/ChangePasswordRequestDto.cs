using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class ChangePasswordRequestDto
    {
        [Required]
        public required string Password { get; set; }

        [Required]
        public required string NewPassword { get; set; }
    }
}
