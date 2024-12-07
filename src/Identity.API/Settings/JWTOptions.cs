using System.ComponentModel.DataAnnotations;

namespace Identity.API.Settings
{
    public class JWTOptions
    {
        [Required]
        public string Issuer { get; set; } = default!;

        [Required]
        [MinLength(1)]
        public string[] Audiences { get; set; } = default!;

        [Required]
        public string Key { get; set; } = default!;

        [Range(1, int.MaxValue, ErrorMessage = "ExpiresInMinutes must be greater than 0.")]
        public int ExpiresInMinutes { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "RefreshTokenExpiriesInMinutes must be greater than 0.")]
        public int RefreshTokenExpiriesInMinutes { get; set; }
    }

}
