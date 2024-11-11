using Identity.Core;
using Identity.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure
{
    public static class DI
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["DefaultConnection"] ?? throw new ArgumentException("connectionString is empty");

            services.AddDbContext<IdentityApplicationDbContext>(
                options => options.UseSqlite(connectionString));

            services.AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<IdentityApplicationDbContext>()
                .AddRoles<IdentityRole>();
         
            return services;
        }
    }
}
