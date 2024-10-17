using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasMany(x => x.VehiclePersons).WithOne(x => x.Vehicle).HasForeignKey(x => x.VehicleId);
            builder.HasMany(x => x.CaseVehicles).WithOne(x => x.Vehicle).HasForeignKey(x => x.VehicleId);
            builder.HasMany(x => x.CrimeSceneVehicles).WithOne(x => x.Vehicle).HasForeignKey(x => x.VehicleId);
        }
    }
}
