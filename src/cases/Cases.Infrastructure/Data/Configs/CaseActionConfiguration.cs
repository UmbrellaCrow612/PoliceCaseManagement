using Cases.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cases.Infrastructure.Data.Configs
{
    public class CaseActionConfiguration : IEntityTypeConfiguration<CaseAction>
    {
        public void Configure(EntityTypeBuilder<CaseAction> builder)
        {
            builder.HasIndex(x => x.Id).IsUnique();

            builder.Property(x => x.CaseId).IsRequired();

            builder.HasIndex(x => x.CreatedById);
        }
    }
}
