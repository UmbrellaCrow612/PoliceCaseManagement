using Evidence.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evidence.Infrastructure.Configs
{
    internal class TagDbConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
