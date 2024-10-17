using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations.Joins
{
    public class CaseDocumentConfiguration : IEntityTypeConfiguration<CaseDocument>
    {
        public void Configure(EntityTypeBuilder<CaseDocument> builder)
        {
            builder.HasKey(x => new { x.CaseId, x.DocumentId });

            builder.HasOne(x => x.Case).WithMany(x => x.CaseDocuments).HasForeignKey(x => x.CaseId);
            builder.HasOne(x => x.Document).WithMany(x => x.CaseDocuments).HasForeignKey(x => x.DocumentId);
        }
    }
}
