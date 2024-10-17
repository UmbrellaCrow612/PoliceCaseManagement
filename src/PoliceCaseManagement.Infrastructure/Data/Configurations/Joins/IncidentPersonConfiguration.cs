using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations.Joins
{
    public class IncidentPersonConfiguration : IEntityTypeConfiguration<IncidentPerson>
    {
        public void Configure(EntityTypeBuilder<IncidentPerson> builder)
        {
            builder.HasKey(x => new { x.IncidentId, x.PersonId });

            builder.HasOne(x => x.Incident).WithMany(x => x.IncidentPersons).HasForeignKey(x => x.IncidentId);
            builder.HasOne(x => x.Person).WithMany(x => x.IncidentPersons).HasForeignKey(x => x.PersonId);
        }
    }
}
