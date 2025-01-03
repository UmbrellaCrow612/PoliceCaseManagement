using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Challenge.Core.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Challenge.Core.Annotations
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class ChallengeAttribute(params string[] claimNames) : Attribute, IAuthorizationFilter
    {
        private readonly string[] _claimNames = claimNames;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            IServiceProvider services = context.HttpContext.RequestServices;

            var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId is null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var options = services.GetService<IOptions<ChallengeJwtSettings>>();
            if (options is null)
            {
                context.Result = new ConflictResult();
                return;
            }

            ChallengeJwtSettings settings = options.Value;

            var requestCookies = context.HttpContext.Request.Cookies;

            // Get all claim cookies from the request
            var claimCookies = _claimNames
                .Where(claim => requestCookies.ContainsKey(claim))
                .ToDictionary(claim => claim, claim => requestCookies[claim]);

            if (claimCookies.Count != _claimNames.Length)
            {
                // If any claim cookie is missing, return Unauthorized
                context.Result = new ConflictResult();
                return;
            }

            foreach (var claimCookie in claimCookies)
            {
                var jwtToken = claimCookie.Value;
                var name = claimCookie.Key;

                if (!ValidateJwtToken(jwtToken,name,userId,settings))
                {
                    // If JWT validation fails, return Unauthorized
                    context.Result = new ConflictResult();
                    return;
                }
            }
        }

        private static bool ValidateJwtToken(string jwtToken, string claimName,string userId, ChallengeJwtSettings settings)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(settings.Key);

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = settings.Issuer,
                    ValidAudiences = settings.Audiences,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out var validatedToken);

                if (validatedToken is not JwtSecurityToken jwtSecurityToken)
                {
                    return false;
                }

                var challengeClaim = principal.FindFirst(JwtRegisteredChallengeClaimNames.ChallengeClaimName)?.Value;
                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return challengeClaim != null && challengeClaim == claimName && userIdClaim != null && userIdClaim == userId;
            }
            catch(Exception e)
            {
                // If validation fails, return false
                return false;
            }
        }
    }
}
