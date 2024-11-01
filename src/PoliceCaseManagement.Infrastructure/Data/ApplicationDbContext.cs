using Microsoft.EntityFrameworkCore;
using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Core.Entities.Joins;
using PoliceCaseManagement.Infrastructure.Data.Seeding;
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
        public DbSet<CrimeScene> CrimeScenes { get; set;  }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Evidence> Evidences { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Statement> Statements { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<CaseDocument> CaseDocuments { get; set; }
        public DbSet<CrimeSceneEvidence> CrimeSceneEvidences { get; set; }
        public DbSet<CrimeScenePerson> CrimeScenePersons { get; set; }
        public DbSet<IncidentPerson> IncidentPersons { get; set; }
        public DbSet<PropertyPerson> PropertyPersons { get; set; }
        public DbSet<StatementUser> StatementUsers { get; set; }
        public DbSet<VehiclePerson> VehiclePersons { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                DatabaseSeeder.SeedData(modelBuilder);
            }
        }
    }
}
