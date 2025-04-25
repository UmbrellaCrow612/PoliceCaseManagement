using Events.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Events
{
    public static  class Extensions
    {
        /// <summary>
        /// Adds the rabbit mq section from the config into the DI and runs some validation on it.
        /// </summary>
        public static IServiceCollection AddRabbitMqSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<RabbitMqSettings>()
                .Bind(configuration.GetSection("RabbitMqSettings"))
                .ValidateDataAnnotations() 
                .ValidateOnStart();

            return services;
        }
    }
}
