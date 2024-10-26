using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PoliceCaseManagement.Application.Interfaces;
using PoliceCaseManagement.Application.Services;

namespace PoliceCaseManagement.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<ICaseService, CaseService>();

            return services;
        }
    }
}
