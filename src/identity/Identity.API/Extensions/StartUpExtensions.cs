using Authorization;
using Identity.API.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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


            return services;
        }

        public static IServiceCollection AddApplicationCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<CORSConfigSettings>()
                 .Bind(configuration.GetSection("Cors"))
                 .ValidateDataAnnotations()
                 .Validate(x => x.Policies.Count > 0, "At least one CORS policy must be defined.")
                 .Validate(x => x.Policies.All(p => p.Value.Origins.Count > 0), "Each CORS policy must have at least one allowed origin.")
                 .Validate(x => x.Policies.All(p => p.Value.Origins.All(origin =>
                     Uri.TryCreate(origin, UriKind.Absolute, out var uri) &&
                     (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))),
                     "All CORS origins must be valid HTTP or HTTPS URLs.")
                 .ValidateOnStart();

            var corsConfig = configuration.GetSection("Cors").Get<CORSConfigSettings>()
                  ?? throw new ApplicationException("Cors config missing");

            foreach (var policy in corsConfig.Policies)
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(policy.Key, builder =>
                    {
                        builder.WithOrigins([.. policy.Value.Origins]);

                        if (policy.Value.AllowAnyHeader)
                        {
                            builder.AllowAnyHeader();
                        }

                        if (policy.Value.AllowAnyMethod)
                        {
                            builder.AllowAnyMethod();
                        }

                        if (policy.Value.AllowCredentials)
                        {
                            builder.AllowCredentials();
                        }
                    });
                });
            }

            return services;
        }
    }
}
