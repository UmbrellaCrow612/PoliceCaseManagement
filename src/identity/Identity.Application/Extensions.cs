using Identity.Application.Helpers;
using Identity.Application.Implementations;
using Identity.Application.Settings;
using Identity.Core.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Events.Core;
using Events.Core.Settings;

namespace Identity.Application
{
    public static class Extensions
    {
        /// <summary>
        /// Adds the application service implementations from the service interfaces and other options
        /// </summary>
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<JwtBearerOptions>()
               .Bind(configuration.GetSection("Jwt"))
               .ValidateDataAnnotations()
               .ValidateOnStart();

            services.AddOptions<TimeWindows>()
                .Bind(configuration.GetSection("TimeWindows"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddScoped<IAuthService, AuthServiceImpl>();
            services.AddScoped<IUserService, UserServiceImpl>();
            services.AddScoped<IDeviceService, DeviceServiceImpl>();
            services.AddScoped<IMfaService, MfaServiceImpl>();
            services.AddScoped<IRoleService, RoleServiceImpl>();
            services.AddScoped<ITokenService, TokenServiceImpl>();
            services.AddScoped<IDeviceVerificationService, DeviceVerificationServiceImpl>();
            services.AddScoped<IUserVerificationService, UserVerificationServiceImpl>();

            services.AddSingleton<IDeviceIdentificationGenerator, DeviceIdentificationGeneratorImpl>();
            services.AddSingleton<ICodeGenerator, CodeGeneratorImpl>();
            services.AddSingleton<JwtBearerHelper>();


            services.AddRabbitMqSettings(configuration);
            var rabbitMqSettings = configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>()
                    ?? throw new ApplicationException("RabbitMqSettings config missing");

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqSettings.Host, "/", h =>
                    {
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
