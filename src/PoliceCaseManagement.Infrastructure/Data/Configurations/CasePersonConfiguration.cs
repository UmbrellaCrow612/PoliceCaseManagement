using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations
{
    public class CasePersonConfiguration : IEntityTypeConfiguration<CasePerson>
    {
        public void Configure(EntityTypeBuilder<CasePerson> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasOne(x => x.Case).WithMany(x => x.CasePersons).HasForeignKey(x => x.CaseId);
            builder.HasOne(x => x.Person).WithMany(x => x.CasePersons).HasForeignKey(x => x.PersonId);
        }
    }
}
