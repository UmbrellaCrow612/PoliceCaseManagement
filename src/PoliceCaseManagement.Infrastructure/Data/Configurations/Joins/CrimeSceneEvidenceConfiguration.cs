using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations.Joins
{
    public class CrimeSceneEvidenceConfiguration : IEntityTypeConfiguration<CrimeSceneEvidence>
    {
        public void Configure(EntityTypeBuilder<CrimeSceneEvidence> builder)
        {
            builder.HasKey(x => new { x.CrimeSceneId, x.EvidenceId });

            builder.HasOne(x => x.CrimeScene).WithMany(x => x.CrimeSceneEvidences).HasForeignKey(x => x.CrimeSceneId);
            builder.HasOne(x => x.Evidence).WithMany(x => x.CrimeSceneEvidences).HasForeignKey(x => x.EvidenceId);
        }
    }
}
