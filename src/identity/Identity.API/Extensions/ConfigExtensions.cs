using Identity.API.Settings;

namespace Identity.API.Extensions
{
    public static class ConfigExtensions
    {
        public static IServiceCollection AddTimeWindows(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<TimeWindows>()
             .Bind(configuration.GetSection("TimeWindows"))
             .ValidateDataAnnotations()
             .ValidateOnStart();

            return services;
        }
    }
}
