using Cases.Core.Models.Joins;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cases.Infrastructure.Data.Configs
{
    public class CaseUserConfiguration : IEntityTypeConfiguration<CaseUser>
    {
        public void Configure(EntityTypeBuilder<CaseUser> builder)
        {
            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasIndex(x => x.UserId).IsUnique();
        }
    }
}
