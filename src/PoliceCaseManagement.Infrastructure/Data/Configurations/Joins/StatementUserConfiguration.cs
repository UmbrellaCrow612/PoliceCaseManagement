using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations.Joins
{
    public class StatementUserConfiguration : IEntityTypeConfiguration<StatementUser>
    {
        public void Configure(EntityTypeBuilder<StatementUser> builder)
        {
            builder.HasKey(x => new { x.StatementId, x.UserId });

            builder.HasOne(x => x.Statement).WithMany(x => x.StatementUsers).HasForeignKey(x => x.StatementId);
            builder.HasOne(x => x.User).WithMany(x => x.StatementUsers).HasForeignKey(x => x.UserId);
        }
    }
}
