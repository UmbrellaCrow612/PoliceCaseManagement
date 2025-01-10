using System.ComponentModel.DataAnnotations;

namespace Challenge.Core.Settings
{
    public class ChallengeJwtSettings
    {
        [Required]
        public string Issuer { get; set; }

        [Required]
        public List<string> Audiences { get; set; }

        [Required]
        [MinLength(32)]
        public string Key { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ExpiresInMinutes must be greater than 0")]
        public int ExpiresInMinutes { get; set; }
    }
}
