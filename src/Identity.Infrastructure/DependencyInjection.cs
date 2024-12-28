using Identity.Core.Models;
using Identity.Infrastructure.Data;
using Identity.Infrastructure.Data.Stores;
using Identity.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddOptions<TimeWindows>()
                .Bind(configuration.GetSection("TimeWindows"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddScoped<ITokenStore, TokenStore>();
            services.AddScoped<IPasswordResetAttemptStore, PasswordResetAttemptStore>();
            services.AddScoped<ISecurityAuditStore, SecurityAuditStore>();
            services.AddScoped<IDepartmentStore, DepartmentStore>();
            services.AddScoped<ILoginAttemptStore, LoginAttemptStore>();
            services.AddScoped<IEmailVerificationAttemptStore, EmailVerificationAttemptStore>();
            services.AddScoped<IUserDeviceStore, UserDeviceStore>();
            services.AddScoped<IUserDeviceChallengeAttemptStore, UserDeviceChallengeAttemptStore>();
            services.AddScoped<IPhoneConfirmationAttemptStore, PhoneConfirmationAttemptStore>();
            services.AddScoped<ITwoFactorCodeAttemptStore, TwoFactorCodeAttemptStore>();

            return services;
        }
    }
}
