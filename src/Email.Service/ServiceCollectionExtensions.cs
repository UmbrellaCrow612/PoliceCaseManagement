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
            services.Configure<EmailOptions>(configuration.GetSection("EmailOptions") ?? throw new ApplicationException("Email settings not set in settings json"));

            return services;
        }
    }
}
