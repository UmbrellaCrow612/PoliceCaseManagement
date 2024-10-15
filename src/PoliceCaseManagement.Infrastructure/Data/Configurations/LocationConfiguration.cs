using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasMany(x => x.Properties).WithOne(x => x.Location).HasForeignKey(x => x.LocationId);
            builder.HasMany(x => x.CrimeScenes).WithOne(x => x.Location).HasForeignKey(x => x.LocationId);
            builder.HasMany(x => x.Incidents).WithOne(x => x.Location).HasForeignKey(x => x.LocationId);
        }
    }
}
