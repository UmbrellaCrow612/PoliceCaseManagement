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
        /// Adds the application service implementations from the service interfaces and other options as well as other stuff - use for API's or other high level stuff
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
            services.AddScoped<ITotpService, TotpServiceImpl>();

            services.AddSingleton<IDeviceIdentificationGenerator, DeviceIdentificationGeneratorImpl>();
            services.AddSingleton<ICodeGenerator, CodeGeneratorImpl>();
            services.AddSingleton<JwtBearerHelper>();
            services.AddSingleton<IPasswordHasher, PasswordHasherImpl>();

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


        /// <summary>
        /// Adds all services to the DI required for the console CLI app to work - mainly used to seed users into the DB and roles
        /// </summary>
        public static IServiceCollection AddConsoleApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserServiceImpl>();
            services.AddScoped<IUserVerificationService, UserVerificationServiceImpl>();
            services.AddSingleton<IPasswordHasher, PasswordHasherImpl>();
            services.AddScoped<IRoleService, RoleServiceImpl>();

            return services;
        }
    }
}
