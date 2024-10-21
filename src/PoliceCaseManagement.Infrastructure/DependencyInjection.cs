using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Core.Interfaces;
using PoliceCaseManagement.Infrastructure.Data;
using PoliceCaseManagement.Infrastructure.Data.Repositories;

namespace PoliceCaseManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICaseRepository<Case, string>, CaseRepository>();

            return services;
        }
    }
}
