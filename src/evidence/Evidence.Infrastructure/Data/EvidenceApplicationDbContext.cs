using Evidence.Core.Models;
using Evidence.Core.Models.Joins;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Evidence.Infrastructure.Data
{
    /// <summary>
    /// EF core db context
    /// </summary>
    public class EvidenceApplicationDbContext(DbContextOptions<EvidenceApplicationDbContext> options) : DbContext(options)
    {

        public DbSet<Core.Models.Evidence> Evidences { get; set; }

        public DbSet<Tag>  Tags { get; set; }

        public DbSet<EvidenceTag> EvidenceTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
