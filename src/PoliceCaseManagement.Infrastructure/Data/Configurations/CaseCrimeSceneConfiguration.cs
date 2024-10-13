using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations
{
    public class CaseCrimeSceneConfiguration : IEntityTypeConfiguration<CaseCrimeScene>
    {
        public void Configure(EntityTypeBuilder<CaseCrimeScene> builder)
        {
            builder.HasKey(x => new {x.CaseId, x.CrimeSceneId });

            builder.HasOne(x => x.Case).WithMany(x => x.CaseCrimeScenes).HasForeignKey(x => x.CaseId);
            builder.HasOne(x => x.CrimeScene).WithMany(x => x.CaseCrimeScenes).HasForeignKey(x => x.CrimeSceneId);
        }
    }
}
