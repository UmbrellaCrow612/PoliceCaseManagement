using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations
{
    public class CrimeSceneConfiguration : IEntityTypeConfiguration<CrimeScene>
    {
        public void Configure(EntityTypeBuilder<CrimeScene> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasOne(x => x.Location).WithMany(x => x.CrimeScenes).HasForeignKey(x => x.LocationId);
            builder.HasOne(x => x.DeletedBy).WithMany(x => x.DeletedCrimeScenes).HasForeignKey(x => x.DeletedById);
            builder.HasMany(x => x.CrimeSceneEvidences).WithOne(x => x.CrimeScene).HasForeignKey(x => x.CrimeSceneId);
            builder.HasMany(x => x.CaseCrimeScenes).WithOne(x => x.CrimeScene).HasForeignKey(x => x.CrimeSceneId);

        }
    }
}
