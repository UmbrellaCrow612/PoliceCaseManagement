using Evidence.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evidence.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString =
                configuration.GetConnectionString("DefaultConnection")
                    ?? throw new ApplicationException("Connection string");

            services.AddDbContext<EvidenceApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            return services;
        }
    }
}
