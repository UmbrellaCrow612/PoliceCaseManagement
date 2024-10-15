using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations
{
    public class IncidentConfiguration : IEntityTypeConfiguration<Incident>
    {
        public void Configure(EntityTypeBuilder<Incident> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasOne(x => x.Location).WithMany(x => x.Incidents).HasForeignKey(x => x.LocationId);
            builder.HasMany(x => x.IncidentPersons).WithOne(x => x.Incident).HasForeignKey(x => x.IncidentId);
        }
    }
}
