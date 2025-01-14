using Identity.API.Services;
using Identity.API.Services.Interfaces;

namespace Identity.API
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
