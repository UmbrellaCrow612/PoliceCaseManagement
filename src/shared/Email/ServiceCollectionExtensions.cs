using Email.Implementations;
using Email.Interfaces;
using Email.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Email
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
