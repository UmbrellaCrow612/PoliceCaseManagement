using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs
{
    public class GenerateChallengeTokenDto
    {
        [Required]
        public required string Password { get; set; }

        [Required]
        public required string ClaimsScope { get; set; }
    }
}
