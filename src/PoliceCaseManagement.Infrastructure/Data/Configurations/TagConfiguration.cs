using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasMany(x => x.CaseTags).WithOne(x => x.Tag).HasForeignKey(x => x.TagId);
        }
    }
}
