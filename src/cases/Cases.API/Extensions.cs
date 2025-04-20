using Authorization;
using Cases.API.DTOs.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Cases.API
{
    public static class Extensions
    {
        public static IServiceCollection AddBaseAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt");
            var key = jwtSettings["Key"] ?? throw new ApplicationException("Jwt Key not provided");
            var issuer = jwtSettings["Issuer"] ?? throw new ApplicationException("Jwt Issuer not provided");
            var audiences = jwtSettings.GetSection("Audiences").Get<string[]>() ?? throw new ApplicationException("Jwt Audiences not provided");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer = issuer,
                            ValidAudiences = audiences,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),

                            ClockSkew = TimeSpan.Zero
                        };

                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context =>
                            {
                                if (context.Request.Cookies.TryGetValue(AuthCookieNamesConstant.JWT, out var token))
                                {
                                    context.Token = token;
                                }
                                return Task.CompletedTask;
                            }
                        };
                    });

            services.AddAuthorization();

            return services;
        }


        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<CaseValidator>();
            return services;
        }
    }
}
