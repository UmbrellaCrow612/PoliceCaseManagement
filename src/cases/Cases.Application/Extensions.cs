using Cases.Application.Implementations;
using Cases.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cases.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddCasesApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICaseService, CaseService>();

            return services;
        }
    }
}
