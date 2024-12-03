using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SMS.Service.Implementations;
using SMS.Service.Settings;

namespace SMS.Service
{
    public static class Extensions
    {
        public static IServiceCollection AddSmsService(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<SmsSettings>(configuration.GetSection("SmsSettings").Get<SmsSettings>() ?? throw new ApplicationException("SmsSettings not present."));

            services.AddSingleton<TwilioSmsService>();

            return services;
        }
    }
}
