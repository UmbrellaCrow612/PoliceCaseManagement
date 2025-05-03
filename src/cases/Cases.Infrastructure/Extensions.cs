using Cases.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cases.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddCasesInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            }

            services.AddDbContext<CasesApplicationDbContext>(option =>
            {
                option.UseNpgsql(connectionString);
            });

            return services;
        }
    }
}
