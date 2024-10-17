using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations.Joins
{
    public class CrimeSceneVehicleConfiguration : IEntityTypeConfiguration<CrimeSceneVehicle>
    {
        public void Configure(EntityTypeBuilder<CrimeSceneVehicle> builder)
        {
            builder.HasKey(x => new { x.CrimeSceneId, x.VehicleId });

            builder.HasOne(x => x.CrimeScene).WithMany(x => x.CrimeSceneVehicles).HasForeignKey(x => x.CrimeSceneId);
            builder.HasOne(x => x.Vehicle).WithMany(x => x.CrimeSceneVehicles).HasForeignKey(x => x.VehicleId);
        }
    }
}
