using Identity.Core;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.API.Helpers
{
    public class JwtHelper(IConfiguration configuration)
    {
        public string GenerateToken(ApplicationUser user, IList<string> roles)
        {
            var handler = new JwtSecurityTokenHandler();

            var key = configuration["Jwt:Key"] ?? throw new ApplicationException("Jwt Key Not Provided");
            var issuer = configuration["Jwt:Issuer"] ?? throw new ApplicationException("Jwt Issuer Not Provided");
            var audience = configuration["Jwt:Audience"] ?? throw new ApplicationException("Jwt Audience Not Provided");
            int expiresInMinutes = int.Parse(configuration["Jwt:ExpiresInMinutes"] ?? throw new ApplicationException("Jwt ExpiresInMinutes Not Provided"));

            var expires = DateTime.UtcNow.AddMinutes(expiresInMinutes);

            var privateKey = Encoding.UTF8.GetBytes(key);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(privateKey), SecurityAlgorithms.HmacSha256);

            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName!));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email!));

            foreach (var role in roles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = credentials,
                Expires = expires,
                Subject = claimsIdentity,
                NotBefore = DateTime.UtcNow,
                IssuedAt = DateTime.UtcNow,
            };

            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }
    }
}
