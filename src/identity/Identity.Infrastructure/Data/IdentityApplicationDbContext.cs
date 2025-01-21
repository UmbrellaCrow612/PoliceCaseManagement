using Challenge.Core.Models;
using Identity.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Identity.Infrastructure.Data
{
    public class IdentityApplicationDbContext(DbContextOptions<IdentityApplicationDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
    {
        public DbSet<TwoFactorSmsAttempt> TwoFactorSmsAttempts { get; set; }
        public DbSet<PhoneConfirmationAttempt> PhoneConfirmationAttempts { get; set; }
        public DbSet<UserDeviceChallengeAttempt> UserDeviceChallengeAttempts { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }
        public DbSet<EmailVerificationAttempt> EmailVerificationAttempts { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<LoginAttempt> LoginAttempts { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<PasswordResetAttempt> PasswordResetAttempts { get; set; }
        public DbSet<SecurityAudit> SecurityAudits { get; set; }
        public DbSet<TwoFactorEmailAttempt> TwoFactorEmailAttempts { get; set; }
        public DbSet<ChallengeClaim> ChallengeClaims { get; set; }
        public DbSet<ChallengeToken> ChallengeTokens { get; set; }
        public DbSet<MagicLinkAttempt> MagicLinkAttempts { get; set; }
        public DbSet<OTPAttempt> OTPAttempts { get; set; }
        public DbSet<TimeBasedOneTimePassCode> TimeBasedOneTimePassCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
