using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations.Joins
{
    public class VehiclePersonConfiguration : IEntityTypeConfiguration<VehiclePerson>
    {
        public void Configure(EntityTypeBuilder<VehiclePerson> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasOne(x => x.Vehicle).WithMany(x => x.VehiclePersons).HasForeignKey(x => x.VehicleId);
            builder.HasOne(x => x.Person).WithMany(x => x.VehiclePersons).HasForeignKey(x => x.PersonId);
        }
    }
}
