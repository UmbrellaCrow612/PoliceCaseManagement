using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations.Joins
{
    public class CaseTagConfiguration : IEntityTypeConfiguration<CaseTag>
    {
        public void Configure(EntityTypeBuilder<CaseTag> builder)
        {
            builder.HasKey(x => new { x.CaseId, x.TagId });

            builder.HasOne(x => x.Case).WithMany(x => x.CaseTags).HasForeignKey(x => x.CaseId);
            builder.HasOne(x => x.Tag).WithMany(x => x.CaseTags).HasForeignKey(x => x.TagId);
        }
    }
}
