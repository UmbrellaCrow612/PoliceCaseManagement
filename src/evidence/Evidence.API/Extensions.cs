using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Evidence.API
{
    public static class Extensions
    {
        /// <summary>
        /// Add auth to the API - JWT auth
        /// </summary>
        public static IServiceCollection AddBaseAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var key = configuration["Jwt:Key"] ?? throw new ApplicationException("JWT Key not found in config");
                var issuer = configuration["Jwt:Issuer"] ?? throw new ApplicationException("JWT issuer not found in config");
                string[] audiences = configuration.GetSection("Jwt:Audiences").Get<string[]>() ?? throw new ApplicationException("Aud JWT not found in config");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudiences = audiences,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });

            return services;
        }
    }
}
