using Cases.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cases.Infrastructure.Data.Configs
{
    public class CasePermissionConfiguration : IEntityTypeConfiguration<CasePermission>
    {
        public void Configure(EntityTypeBuilder<CasePermission> builder)
        {
            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasIndex(x => x.UserId);
        }
    }
}
