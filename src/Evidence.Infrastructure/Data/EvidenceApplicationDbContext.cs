using Evidence.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Evidence.Infrastructure.Data
{
    public class EvidenceApplicationDbContext : DbContext
    {
        public DbSet<EvidenceItem> Evidences { get; set; }
        public DbSet<CustodyLog> CustodyLogs { get; set; }
        public DbSet<LabResult> LabResults { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
