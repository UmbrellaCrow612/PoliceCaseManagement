using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations
{
    public class PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasOne(x => x.Location).WithMany(x => x.Properties).HasForeignKey(x => x.LocationId);
            builder.HasMany(x => x.PropertyPersons).WithOne(x => x.Property).HasForeignKey(x => x.PropertyId);
        }
    }
}
