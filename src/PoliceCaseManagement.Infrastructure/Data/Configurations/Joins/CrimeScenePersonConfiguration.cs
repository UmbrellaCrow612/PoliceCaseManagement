using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations.Joins
{
    public class CrimeScenePersonConfiguration : IEntityTypeConfiguration<CrimeScenePerson>
    {
        public void Configure(EntityTypeBuilder<CrimeScenePerson> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasOne(x => x.CrimeScene).WithMany(x => x.CrimeScenePersons).HasForeignKey(x => x.CrimeSceneId);
            builder.HasOne(x => x.Person).WithMany(x => x.CrimeScenePersons).HasForeignKey(x => x.PersonId);
        }
    }
}
