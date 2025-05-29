using Cases.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cases.Infrastructure.Data.Configs
{
    public class CaseAttachmentFileConfiguration : IEntityTypeConfiguration<CaseAttachmentFile>
    {
        public void Configure(EntityTypeBuilder<CaseAttachmentFile> builder)
        {
            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
