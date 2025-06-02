using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Identity.API.Extensions
{
    public static class StartUpExtensions
    {
        public static IServiceCollection AddBaseAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer = configuration["Jwt:Issuer"] ?? throw new ApplicationException("Jwt Issuer not provided"),
                            ValidAudiences = configuration.GetSection("Jwt:Audiences").Get<string[]>(),
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? throw new ApplicationException("Jwt Key not provided"))),

                            ClockSkew = TimeSpan.Zero
                        };
                    });

            return services;
        }
    }
}
