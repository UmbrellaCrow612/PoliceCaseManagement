using System.ComponentModel.DataAnnotations;

namespace Identity.Application.Helpers
{
    /// <summary>
    /// Configuration options for JSON Web Token (JWT) authentication and authorization.
    /// </summary>
    public class JwtBearerOptions
    {
        /// <summary>
        /// The issuer of the JWT token, typically representing the authentication server or application.
        /// </summary>
        /// <remarks>
        /// This is a required field and must be a valid string identifying the token issuer.
        /// </remarks>
        [Required]
        public string Issuer { get; set; } = default!;

        /// <summary>
        /// An array of valid audiences (recipients) for the JWT token.
        /// </summary>
        /// <remarks>
        /// At least one audience must be specified. These represent the intended recipients of the token.
        /// </remarks>
        [Required]
        [MinLength(1)]
        public string[] Audiences { get; set; } = default!;

        /// <summary>
        /// The secret key used for signing and validating JWT tokens.
        /// </summary>
        /// <remarks>
        /// This is a critical security parameter and should be kept confidential.
        /// </remarks>
        [Required]
        public string Key { get; set; } = default!;

        /// <summary>
        /// The duration for which the JWT token remains valid, specified in minutes.
        /// </summary>
        /// <remarks>
        /// Must be a positive integer representing the token's lifetime.
        /// </remarks>
        [Range(1, int.MaxValue, ErrorMessage = "ExpiresInMinutes must be greater than 0.")]
        public int ExpiresInMinutes { get; set; }

        /// <summary>
        /// The duration for which the refresh token remains valid, specified in minutes.
        /// </summary>
        /// <remarks>
        /// Must be a positive integer representing the refresh token's lifetime.
        /// </remarks>
        [Range(1, int.MaxValue, ErrorMessage = "RefreshTokenExpiriesInMinutes must be greater than 0.")]
        public int RefreshTokenExpiriesInMinutes { get; set; }
    }
}
