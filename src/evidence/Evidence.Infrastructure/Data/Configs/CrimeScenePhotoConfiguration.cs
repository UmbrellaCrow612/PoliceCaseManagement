using Evidence.Infrastructure.Data.Models.Joins;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evidence.Infrastructure.Data.Configs
{
    internal class CrimeScenePhotoConfiguration : IEntityTypeConfiguration<CrimeScenePhoto>
    {
        public void Configure(EntityTypeBuilder<CrimeScenePhoto> builder)
        {
            builder.HasKey(x => new { x.CrimeSceneId, x.PhotoId });

            builder.HasOne(x => x.CrimeScene).WithMany(x => x.CrimeScenePhotos).HasForeignKey(x => x.CrimeSceneId);

            builder.HasOne(x => x.Photo).WithMany(x => x.CrimeScenePhotos).HasForeignKey(x => x.PhotoId);
        }
    }
}
