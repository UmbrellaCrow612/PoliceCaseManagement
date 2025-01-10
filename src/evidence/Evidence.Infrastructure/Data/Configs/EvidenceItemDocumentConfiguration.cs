using Evidence.Infrastructure.Data.Models.Joins;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evidence.Infrastructure.Data.Configs
{
    internal class EvidenceItemDocumentConfiguration : IEntityTypeConfiguration<EvidenceItemDocument>
    {
        public void Configure(EntityTypeBuilder<EvidenceItemDocument> builder)
        {
            builder.HasKey(x => new { x.EvidenceItemId, x.DocumentId });
        }
    }
}
