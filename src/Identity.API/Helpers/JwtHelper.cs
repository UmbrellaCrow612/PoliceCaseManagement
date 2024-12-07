using Identity.API.Settings;
using Identity.Infrastructure.Data.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Identity.API.Helpers
{
    public class JwtHelper(IOptions<JWTOptions> JWTOptions)
    {
        private readonly JWTOptions _JWTOptions = JWTOptions.Value;

        /// <summary>
        /// Generate a JWT token with claims
        /// </summary>
        /// <returns>
        /// Access token and it's ID
        /// </returns>
        public (string accessToken, string accessTokenId) GenerateToken(ApplicationUser user, IList<string> roles)
        {
            var handler = new JwtSecurityTokenHandler();

            var key = _JWTOptions.Key;
            var issuer = _JWTOptions.Issuer;
            string[] audiences = _JWTOptions.Audiences;
            int expiresInMinutes = _JWTOptions.ExpiresInMinutes;
            var expires = DateTime.UtcNow.AddMinutes(expiresInMinutes);
            var privateKey = Encoding.UTF8.GetBytes(key);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(privateKey), SecurityAlgorithms.HmacSha256);
            var tokenId = Guid.NewGuid().ToString();

            var claimsIdentity = new ClaimsIdentity();

            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, tokenId));

            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName!));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email!));

            foreach (string aud in audiences)
            {
                claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Aud, aud));
            }

            foreach (var role in roles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                SigningCredentials = credentials,
                Expires = expires,
                Subject = claimsIdentity,
                NotBefore = DateTime.UtcNow,
                IssuedAt = DateTime.UtcNow,
            };

            var token = handler.CreateToken(tokenDescriptor);
            var tokenValue = handler.WriteToken(token);

            return (tokenValue, tokenId);
        }

        public string GenerateRefreshToken()
        {
            byte[] randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return Convert.ToBase64String(randomBytes);
        }
    }
}
