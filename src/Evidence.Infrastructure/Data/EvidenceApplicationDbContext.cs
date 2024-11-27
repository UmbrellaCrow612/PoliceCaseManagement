using Evidence.Infrastructure.Data.Models;
using Evidence.Infrastructure.Data.Models.Joins;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Evidence.Infrastructure.Data
{
    public class EvidenceApplicationDbContext(DbContextOptions<EvidenceApplicationDbContext> options) : DbContext(options)
    {
        public required DbSet<CrimeSceneEvidence> CrimeSceneEvidences { get; set; }
        public required DbSet<CrimeScene> CrimeScenes { get; set; }
        public required DbSet<EvidenceItem> Evidences { get; set; }
        public required DbSet<CustodyLog> CustodyLogs { get; set; }
        public required DbSet<LabResult> LabResults { get; set; }
        public required DbSet<Note> Notes { get; set; }
        public required DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
