using Microsoft.EntityFrameworkCore;
using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Core.Entities.Joins;
using System.Reflection;

namespace PoliceCaseManagement.Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Case> Cases { get; set; }
        public DbSet<CasePerson> CasePersons { get; set; }
        public DbSet<CaseUser> CaseUsers { get; set; }
        public DbSet<CaseTag> CaseTags { get; set; }
        public DbSet<CaseEvidence> CaseEvidences { get; set; }
        public DbSet<CaseCrimeScene> CaseCrimeScenes { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
