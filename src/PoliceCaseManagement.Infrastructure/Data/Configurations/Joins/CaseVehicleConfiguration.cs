using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations.Joins
{
    public class CaseVehicleConfiguration : IEntityTypeConfiguration<CaseVehicle>
    {
        public void Configure(EntityTypeBuilder<CaseVehicle> builder)
        {
            builder.HasKey(x => new { x.CaseId, x.VehicleId });

            builder.HasOne(x => x.Case).WithMany(x => x.CaseVehicles).HasForeignKey(x => x.CaseId);
            builder.HasOne(x => x.Vehicle).WithMany(x => x.CaseVehicles).HasForeignKey(x => x.VehicleId);
        }
    }
}
