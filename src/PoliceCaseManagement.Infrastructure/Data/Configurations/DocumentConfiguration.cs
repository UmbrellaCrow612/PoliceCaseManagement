using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasOne(x => x.CreatedBy).WithMany(x => x.CreatedDocuments).HasForeignKey(x => x.CreatedById);
            builder.HasOne(x => x.LastEditedBy).WithMany(x => x.LastEditedDocuments).HasForeignKey(x => x.LastEditedById);
            builder.HasOne(x => x.DeletedBy).WithMany(x => x.DeletedDocuments).HasForeignKey(x => x.DeletedById);
            builder.HasMany(x => x.CaseDocuments).WithOne(x => x.Document).HasForeignKey(x => x.DocumentId);
        }
    }
}
