using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class ReSendTwoFactorEmailAttemptDto
    {
        [Required]
        public required string LoginAttemptId { get; set; }
    }
}
