using System.Reflection;
using Identity.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data
{
    public class IdentityApplicationDbContext(DbContextOptions<IdentityApplicationDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
    {
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
