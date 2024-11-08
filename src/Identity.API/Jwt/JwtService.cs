using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Api.Jwt
{
    /// <summary>
    /// Helper to handle JWT authentication in the Identity API
    /// </summary>
    public class JwtService(IOptions<JwtSettings> jwtSettings)
    {
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;


        /// <summary>
        /// Generates both access and refresh tokens for a user.
        /// </summary>
        /// <param name="claims">User claims to include in the token.</param>
        /// <returns>A tuple containing the access token and refresh token.</returns>
        public (string AccessToken, string RefreshToken) GenerateTokens(IEnumerable<Claim> claims)
        {
            var accessToken = GenerateAccessToken(claims);
            var refreshToken = GenerateRefreshToken();
            return (accessToken, refreshToken);
        }

        /// <summary>
        /// Generates a new access token based on the provided claims.
        /// </summary>
        /// <param name="claims">User claims to include in the token.</param>
        /// <returns>A signed JWT access token.</returns>
        private string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Generates a cryptographically secure refresh token.
        /// </summary>
        /// <returns>A base64 encoded refresh token.</returns>
        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        /// <summary>
        /// Validates an access token and returns the principal if valid.
        /// </summary>
        /// <param name="token">The JWT token to validate.</param>
        /// <returns>The validated ClaimsPrincipal or null if invalid.</returns>
        public ClaimsPrincipal? ValidateAccessToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidAudience = _jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return principal;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Refreshes an access token using a valid refresh token.
        /// </summary>
        /// <param name="expiredToken">The expired access token.</param>
        /// <param name="refreshToken">The refresh token.</param>
        /// <param name="validateRefreshToken">Function to validate the refresh token against storage.</param>
        /// <returns>A new token pair if successful, null if refresh fails.</returns>
        public async Task<(string AccessToken, string RefreshToken)?> RefreshTokenAsync(
            string expiredToken,
            string refreshToken,
            Func<string, Task<bool>> validateRefreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

            // Validate the expired token's signature and get claims
            try
            {
                var principal = tokenHandler.ValidateToken(expiredToken, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false, // Don't validate lifetime as token is expired
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidAudience = _jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                // Validate the refresh token
                if (!await validateRefreshToken(refreshToken))
                {
                    return null;
                }

                // Generate new tokens
                var claims = principal.Claims;
                return GenerateTokens(claims);
            }
            catch
            {
                return null;
            }
        }
    }
}
