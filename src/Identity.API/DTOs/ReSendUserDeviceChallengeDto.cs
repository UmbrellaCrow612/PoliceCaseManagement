using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class ReSendUserDeviceChallengeDto
    {
        [EmailAddress]
        [Required]
        public required string Email { get; set; }
    }
}
