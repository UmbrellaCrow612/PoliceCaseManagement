using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations
{
    public class CaseUserConfiguration : IEntityTypeConfiguration<CaseUser>
    {
        public void Configure(EntityTypeBuilder<CaseUser> builder)
        {
            builder.HasKey(x => new {x.UserId, x.CaseId });

            builder.HasOne(x => x.User).WithMany(x => x.CaseUsers).HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.Case).WithMany(x => x.CaseUsers).HasForeignKey(x => x.CaseId);
        }
    }
}
