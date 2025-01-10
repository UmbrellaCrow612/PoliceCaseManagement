using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class UserDeviceChallengeDto
    {
        [EmailAddress]
        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Code { get; set; }
    }
}
