using Identity.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Identity.Infrastructure.Data
{
    public class IdentityApplicationDbContext(DbContextOptions<IdentityApplicationDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
    {
        public required DbSet<TwoFactorCodeAttempt> TwoFactorCodeAttempts { get; set; }
        public required DbSet<PhoneConfirmationAttempt> PhoneConfirmationAttempts { get; set; }
        public required DbSet<UserDeviceChallengeAttempt> UserDeviceChallengeAttempts { get; set; }
        public required DbSet<UserDevice> UserDevices { get; set; }
        public required DbSet<EmailVerificationAttempt> EmailVerificationAttempts { get; set; }
        public required DbSet<Department> Departments { get; set; }
        public required DbSet<LoginAttempt> LoginAttempts { get; set; }
        public required DbSet<Token> Tokens { get; set; }
        public required DbSet<PasswordResetAttempt> PasswordResetAttempts { get; set; }
        public required DbSet<SecurityAudit> SecurityAudits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
