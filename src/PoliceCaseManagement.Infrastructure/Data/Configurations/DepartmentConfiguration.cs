using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasMany(x => x.AssignedCases).WithOne(x => x.Department).HasForeignKey(x => x.DepartmentId);
            builder.HasMany(x => x.AssignedUsers).WithOne(x => x.Department).HasForeignKey(x => x.DepartmentId);
        }
    }
}
