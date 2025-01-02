using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Challenge.Core.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime;

namespace Challenge.Core.Annotations
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class ChallengeAttribute(params string[] claimNames) : Attribute, IAuthorizationFilter
    {
        private readonly string[] _claimNames = claimNames;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            IServiceProvider services = context.HttpContext.RequestServices;

            var options = services.GetService<IOptions<ChallengeJwtSettings>>();
            if (options == null)
            {
                context.Result = new UnauthorizedResult();
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
                context.Result = new UnauthorizedResult();
                return;
            }

            foreach (var claimCookie in claimCookies)
            {
                var jwtToken = claimCookie.Value;

                if (!ValidateJwtToken(jwtToken, settings))
                {
                    // If JWT validation fails, return Unauthorized
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }
        }

        private static bool ValidateJwtToken(string jwtToken, ChallengeJwtSettings settings)
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

                // Validate the token
                tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out var validatedToken);

                // If token is valid, return true
                return true;
            }
            catch(Exception e)
            {
                // If validation fails, return false
                return false;
            }
        }
    }
}
