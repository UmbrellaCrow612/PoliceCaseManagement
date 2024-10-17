using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations
{
    public class EvidenceConfiguration : IEntityTypeConfiguration<Evidence>
    {
        public void Configure(EntityTypeBuilder<Evidence> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasOne(x => x.CreatedBy).WithMany(x => x.CreatedEvidences).HasForeignKey(x => x.CreatedById);
            builder.HasOne(x => x.LastEditedBy).WithMany(x => x.LastEditedEvidences).HasForeignKey(x => x.LastEditedById);
            builder.HasOne(x => x.DeletedBy).WithMany(x => x.DeletedEvidences).HasForeignKey(x => x.DeletedById);

            builder.HasMany(x => x.CaseEvidences).WithOne(x => x.Evidence).HasForeignKey(x => x.EvidenceId);
            builder.HasMany(x => x.CrimeSceneEvidences).WithOne(x => x.Evidence).HasForeignKey(x => x.EvidenceId);
        }
    }
}
