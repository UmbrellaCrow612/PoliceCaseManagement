using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations.Joins
{
    public class PropertyPersonConfiguration : IEntityTypeConfiguration<PropertyPerson>
    {
        public void Configure(EntityTypeBuilder<PropertyPerson> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasOne(x => x.Property).WithMany(x => x.PropertyPersons).HasForeignKey(x => x.PropertyId);
            builder.HasOne(x => x.Person).WithMany(x => x.PropertyPersons).HasForeignKey(x => x.PersonId);
        }
    }
}
