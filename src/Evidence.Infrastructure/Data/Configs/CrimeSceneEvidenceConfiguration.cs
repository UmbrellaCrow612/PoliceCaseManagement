using Evidence.Infrastructure.Data.Models.Joins;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evidence.Infrastructure.Data.Configs
{
    public class CrimeSceneEvidenceConfiguration : IEntityTypeConfiguration<CrimeSceneEvidence>
    {
        public void Configure(EntityTypeBuilder<CrimeSceneEvidence> builder)
        {
            builder.HasKey(x => new { x.CrimeSceneId, x.EvidenceItemId });

            builder.HasOne(x => x.CrimeScene).WithMany(x => x.CrimeSceneEvidences).HasForeignKey(x => x.CrimeSceneId);

            builder.HasOne(x => x.EvidenceItem).WithMany(x => x.CrimeSceneEvidences).HasForeignKey(x => x.EvidenceItemId);
        }
    }
}
