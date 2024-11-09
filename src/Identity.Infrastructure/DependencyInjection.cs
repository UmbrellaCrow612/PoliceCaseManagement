using Identity.Core.Entities;
using Identity.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure
{
    /// <summary>
    /// Add Identity Infrastructure
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found.");
            }

            services.AddDbContext<IdentityApplicationDbContext>(options =>
                options.UseSqlite(connectionString));

            services.AddIdentityCore<ApplicationUser>(options =>
            {
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<IdentityApplicationDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
            });

            return services;
        }
    }
}
