using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CORS
{
    public static class Extensions
    {
        /// <summary>
        /// Custom application cors set up.
        /// </summary>
        public static IServiceCollection AddApplicationCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<CORSConfigSettings>()
                 .Bind(configuration.GetSection(CORSConfigNamesConstant.Name))
                 .ValidateDataAnnotations()
                 .Validate(x => x.Policies.Count > 0, "At least one CORS policy must be defined.")
                 .Validate(x => x.Policies.All(p => p.Value.Origins.Count > 0), "Each CORS policy must have at least one allowed origin.")
                 .Validate(x => x.Policies.All(p => p.Value.Origins.All(origin =>
                     Uri.TryCreate(origin, UriKind.Absolute, out var uri) &&
                     (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))),
                     "All CORS origins must be valid HTTP or HTTPS URLs.")
                 .ValidateOnStart();

            var corsConfig = configuration.GetSection(CORSConfigNamesConstant.Name).Get<CORSConfigSettings>()
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
