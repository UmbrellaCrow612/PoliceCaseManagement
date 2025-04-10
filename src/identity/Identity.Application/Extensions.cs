using Identity.Application.Helpers;
using Identity.Application.Implamentations;
using Identity.Application.Settings;
using Identity.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application
{
    public static class Extensions
    {
        /// <summary>
        /// Adds the application service implamenations from the service interfaces
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

            return services;
        }
    }
}
