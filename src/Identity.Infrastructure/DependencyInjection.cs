using Identity.Core.Entities;
using Identity.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found.");
            }

            services.AddDbContext<IdentityApplicationDbContext>(options =>
                options.UseSqlite(connectionString,
                    b => b.MigrationsAssembly(typeof(IdentityApplicationDbContext).Assembly.FullName)));

            services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddRoleManager<IdentityRole>()
            .AddUserStore<ApplicationUser>()
            .AddUserManager<ApplicationUser>()
            .AddEntityFrameworkStores<IdentityApplicationDbContext>();

            return services;
        }
    }
}
