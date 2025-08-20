using System.Reflection;
using Identity.Core.Models;
using Identity.Core.Models.Joins;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data
{
    /// <summary>
    /// Db context for identity ef core database
    /// </summary>
    public class IdentityApplicationDbContext(DbContextOptions<IdentityApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<ApplicationRole> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<TwoFactorSms> TwoFactorSms { get; set; }
        public DbSet<PhoneVerification> PhoneVerifications { get; set; }
        public DbSet<DeviceVerification> DeviceVerifications { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Token> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
