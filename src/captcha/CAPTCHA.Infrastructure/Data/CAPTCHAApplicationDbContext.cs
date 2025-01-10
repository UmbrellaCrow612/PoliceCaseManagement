using CAPTCHA.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CAPTCHA.Infrastructure.Data
{
    public class CAPTCHAApplicationDbContext(DbContextOptions<CAPTCHAApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<CAPTCHAMathQuestion> CAPTCHAMathQuestions { get; set; }
        public DbSet<CAPTCHAAudioQuestion> CAPTCHAAudioQuestions { get; set; }
        public DbSet<CAPTCHAGridQuestion> CAPTCHAGridParentQuestions { get; set; }
        public DbSet<CAPTCHAGridChild> CAPTCHAGridChildren { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CAPTCHAApplicationDbContext).Assembly);
        }

    }
}
