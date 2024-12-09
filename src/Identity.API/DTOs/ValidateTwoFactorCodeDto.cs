using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class ValidateTwoFactorCodeDto
    {
        [Required]
        public required string LoginAttemptId { get; set; }

        [Required]
        public required string Code { get; set; }
    }
}
