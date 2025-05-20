using Events;
using Events.Settings;
using Identity.Application.Helpers;
using Identity.Application.Implamentations;
using Identity.Application.Settings;
using Identity.Core.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application
{
    public static class Extensions
    {
        /// <summary>
        /// Adds the application service implementations from the service interfaces
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<JwtBearerOptions>()
               .Bind(configuration.GetSection("Jwt"))
               .ValidateDataAnnotations()
               .ValidateOnStart();

            services.AddScoped<JwtBearerHelper>();

            services.AddScoped<IDeviceIdentification, DeviceIdentification>();
            services.AddScoped<DeviceManager>();

            services.AddOptions<TimeWindows>()
                .Bind(configuration.GetSection("TimeWindows"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<PasswordConfigSettings>()
                .Bind(configuration.GetSection("PasswordSettings"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddScoped<IAuthService, AuthService>();


            services.AddRabbitMqSettings(configuration);
            var rabbitMqSettings = configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>()
                    ?? throw new ApplicationException("RabbitMqSettings config missing");

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqSettings.Host, "/", h => { 
                        h.Username(rabbitMqSettings.Username); 
                        h.Password(rabbitMqSettings.Password); 
                    });

                    cfg.ReceiveEndpoint("identity-queue-name", e => // Define a specific queue for this API's consumers
                    {
                     
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
