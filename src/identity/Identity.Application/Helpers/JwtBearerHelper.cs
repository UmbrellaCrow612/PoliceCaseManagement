using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Application.Helpers
{
    public class JwtBearerHelper(IOptions<JwtBearerOptions> JWTOptions)
    {
        private readonly JwtBearerOptions _JWTOptions = JWTOptions.Value;

        /// <summary>
        /// Generate a JWT Bearer token with claims
        /// </summary>
        /// <returns>
        /// Bearer token and it's ID
        /// </returns>
        public string GenerateBearerToken<TUser>(TUser user, IList<string> roles)
        where TUser : IdentityUser
        {
            var handler = new JwtSecurityTokenHandler();

            var key = _JWTOptions.Key;
            var issuer = _JWTOptions.Issuer;
            string[] audiences = _JWTOptions.Audiences;
            int expiresInMinutes = _JWTOptions.ExpiresInMinutes;
            var expires = DateTime.UtcNow.AddMinutes(expiresInMinutes);
            var privateKey = Encoding.UTF8.GetBytes(key);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(privateKey), SecurityAlgorithms.HmacSha256);

            var claimsIdentity = new ClaimsIdentity();

            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
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

            return tokenValue;
        }

        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
