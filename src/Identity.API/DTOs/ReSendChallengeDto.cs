using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class ReSendChallengeDto
    {
        [EmailAddress]
        [Required]
        public required string Email { get; set; }
    }
}
