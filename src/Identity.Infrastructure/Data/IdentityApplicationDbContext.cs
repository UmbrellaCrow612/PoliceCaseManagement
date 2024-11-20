using Identity.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Identity.Infrastructure.Data
{
    public class IdentityApplicationDbContext(DbContextOptions<IdentityApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public required DbSet<LoginAttempt> LoginAttempts { get; set; }
        public required DbSet<Token> Tokens { get; set; }
        public required DbSet<DeviceInfo> DeviceInfos { get; set; }
        public required DbSet<PasswordResetAttempt> PasswordResetAttempts { get; set; }
        public required DbSet<SecurityAudit> SecurityAudits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
