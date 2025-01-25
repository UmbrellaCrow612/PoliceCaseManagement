using Email.Service.Implementations;
using Email.Service.Interfaces;
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
            services.Configure<EmailSettings>(
            configuration.GetSection("EmailSettings"));

            services.AddOptions<EmailSettings>()
                .Bind(configuration.GetSection("EmailSettings"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddScoped<IEmailService, SendGridEmailService>();

            return services;
        }
    }
}
