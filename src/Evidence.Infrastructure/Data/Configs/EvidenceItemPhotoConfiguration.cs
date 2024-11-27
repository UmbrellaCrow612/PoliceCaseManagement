using Evidence.Infrastructure.Data.Models.Joins;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evidence.Infrastructure.Data.Configs
{
    internal class EvidenceItemPhotoConfiguration : IEntityTypeConfiguration<EvidenceItemPhoto>
    {
        public void Configure(EntityTypeBuilder<EvidenceItemPhoto> builder)
        {
            builder.HasKey(x => new { x.EvidenceItemId, x.PhotoId });

            builder.HasOne(x => x.Evidence).WithMany(x => x.EvidenceItemPhotos).HasForeignKey(x => x.EvidenceItemId);

            builder.HasOne(x => x.Photo).WithMany(x => x.EvidenceItemPhotos).HasForeignKey(x => x.PhotoId);
        }
    }
}
