using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class ValidateTwoFactorSmsAttemptDto
    {
        [Required]
        public required string LoginAttemptId { get; set; }

        [Required]
        public required string Code { get; set; }
    }
}
