using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Authorization.Core
{
    public static class Extensions
    {
        /// <summary>
        /// Adds the Authorization to a project
        /// </summary>
        public static IServiceCollection AddBaseAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<JwtBearerOptions>()
                .Bind(configuration.GetSection("Jwt"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddScoped<JwtBearerHelper>();

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


                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context =>
                            {
                                if (context.Request.Cookies.TryGetValue(CookieNamesConstant.JWT, out var token))
                                {
                                    context.Token = token;
                                }
                                return Task.CompletedTask;
                            }
                        };
                    });


            return services;
        }
    }
}
