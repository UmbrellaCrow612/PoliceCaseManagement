using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SMS.Implementations;
using SMS.Settings;

namespace SMS
{
    public static class Extensions
    {
        public static IServiceCollection AddSmsService(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddOptions<SmsSettings>()
                .Bind(configuration.GetSection("SmsSettings"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddSingleton<TwilioSmsService>();

            return services;
        }
    }
}
