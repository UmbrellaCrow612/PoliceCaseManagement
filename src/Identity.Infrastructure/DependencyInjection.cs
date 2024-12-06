using Identity.Infrastructure.Data;
using Identity.Infrastructure.Data.Models;
using Identity.Infrastructure.Data.Stores;
using Identity.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Identity.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentException("DefaultConnection is empty");

            services.AddDbContext<IdentityApplicationDbContext>(
                options => options.UseSqlite(connectionString));

            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<IdentityApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
            });

            services.Configure<TimeWindows>(options =>
            {
                configuration.GetSection("TimeWindows").Bind(options);

                // Validate configuration and throw if invalid
                if (options.ResetPasswordTime <= 0 ||
                    options.EmailConfirmationTime <= 0 ||
                    options.DeviceChallengeTime <= 0 ||
                    options.PhoneConfirmationTime <= 0)
                {
                    throw new ArgumentException("Invalid TimeWindows configuration. All time values must be greater than zero.");
                }
            });

            services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<TimeWindows>>().Value);

            services.AddScoped<ITokenStore, TokenStore>();
            services.AddScoped<IPasswordResetAttemptStore, PasswordResetAttemptStore>();
            services.AddScoped<ISecurityAuditStore, SecurityAuditStore>();
            services.AddScoped<IDepartmentStore, DepartmentStore>();
            services.AddScoped<ILoginAttemptStore, LoginAttemptStore>();
            services.AddScoped<IEmailVerificationAttemptStore, EmailVerificationAttemptStore>();
            services.AddScoped<IUserDeviceStore, UserDeviceStore>();
            services.AddScoped<IUserDeviceChallengeAttemptStore, UserDeviceChallengeAttemptStore>();
            services.AddScoped<IPhoneConfirmationAttemptStore, PhoneConfirmationAttemptStore>();

            return services;
        }
    }
}
