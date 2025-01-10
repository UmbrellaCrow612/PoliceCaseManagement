using Challenge.Core.Models;
using Challenge.Core.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Challenge.Core.Helpers
{
    public class JwtChallengeHelper(IOptions<ChallengeJwtSettings> options)
    {
        private readonly ChallengeJwtSettings _settings = options.Value;

        public string GenerateJwtToken(ChallengeToken token)
        {
            if (token.IsExpired())
            {
                throw new InvalidOperationException("Cannot generate a token for an expired ChallengeToken.");
            }

            List<string> audiences = _settings.Audiences;
            int expiresInMinutes = _settings.ExpiresInMinutes;
            var expires = DateTime.UtcNow.AddMinutes(expiresInMinutes);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims =
            [
                new Claim(JwtRegisteredClaimNames.Jti, token.Id),
                new Claim(JwtRegisteredClaimNames.Sub, token.UserId),
                new Claim(JwtRegisteredChallengeClaimNames.ChallengeClaimId, token.ChallengeClaimId),
                new Claim(JwtRegisteredChallengeClaimNames.ChallengeClaimName, token.ChallengeClaimName),
            ];

            foreach (var aud in audiences)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Aud, aud));
            }


            var jwtToken = new JwtSecurityToken(
               issuer: _settings.Issuer,
               claims: claims,
               notBefore: DateTime.UtcNow,
               expires: expires,
               signingCredentials: credentials
           );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
