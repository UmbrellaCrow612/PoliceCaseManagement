using Email.Service.Implamentations;
using Email.Service.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Email.Service
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEmailService(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<EmailSettings>(configuration.GetSection("EmailSettings").Get<EmailSettings>() ?? throw new ApplicationException("EmailSettings not present."));

            services.AddScoped<GmailService>();

            return services;
        }
    }
}
