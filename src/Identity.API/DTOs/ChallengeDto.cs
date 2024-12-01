using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class ChallengeDto
    {
        [EmailAddress]
        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Code { get; set; }
    }
}
