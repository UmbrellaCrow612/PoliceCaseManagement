using Microsoft.EntityFrameworkCore;
using Reporting.Core.Models;
using System.Reflection;

namespace Reporting.Infrastructure.Data
{
    public class ReportingApplicationDbContext(DbContextOptions<ReportingApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
