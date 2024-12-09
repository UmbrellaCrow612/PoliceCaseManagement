using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class ReSendTwoFactorCode
    {
        [Required]
        public required string LoginAttemptId { get; set; }
    }
}
