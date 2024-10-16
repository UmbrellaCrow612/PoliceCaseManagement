using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations
{
    public class StatementConfiguration : IEntityTypeConfiguration<Statement>
    {
        public void Configure(EntityTypeBuilder<Statement> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasOne(x => x.Person).WithMany(x => x.Statements).HasForeignKey(x => x.PersonId);
            builder.HasMany(x => x.StatementUsers).WithOne(x => x.Statement).HasForeignKey(x => x.StatementId);
            builder.HasOne(x => x.CreatedBy).WithMany(x => x.CreatedStatements).HasForeignKey(x => x.CreatedById);
            builder.HasOne(x => x.LastEditedBy).WithMany(x => x.LastEditedStatements).HasForeignKey(x => x.LastEditedById);
        }
    }
}
