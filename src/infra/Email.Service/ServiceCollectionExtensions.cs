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
            services.AddOptionsWithValidateOnStart<EmailSettings>("EmailSettings")
                .BindConfiguration("EmailSettings")
                .ValidateDataAnnotations();

            services.AddScoped<GmailService>();

            return services;
        }
    }
}
