using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class ValidateTwoFactorEmailAttemptDto
    {
        [Required]
        public required string LoginAttemptId { get; set; }

        [Required]
        public required string EmailCode { get; set; }
    }
}
