using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations
{
    public class CaseEvidenceConfiguration : IEntityTypeConfiguration<CaseEvidence>
    {
        public void Configure(EntityTypeBuilder<CaseEvidence> builder)
        {
            builder.HasKey(x => new {x.CaseId, x.EvidenceId });

            builder.HasOne(x => x.Case).WithMany(x => x.CaseEvidences).HasForeignKey(x => x.CaseId);
            builder.HasOne(x => x.Evidence).WithMany(x => x.CaseEvidences).HasForeignKey(x => x.EvidenceId);
        }
    }
}
