using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasOne(x => x.DeletedBy).WithMany(x => x.DeletedPersons).HasForeignKey(x => x.DeletedById);
            builder.HasMany(x => x.Statements).WithOne(x => x.Person).HasForeignKey(x => x.PersonId);
            builder.HasMany(x => x.CrimeScenePersons).WithOne(x => x.Person).HasForeignKey(x => x.PersonId);
            builder.HasMany(x => x.CasePersons).WithOne(x => x.Person).HasForeignKey(x => x.PersonId);
        }
    }
}
