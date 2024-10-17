using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasOne(x => x.Department).WithMany(x => x.AssignedUsers).HasForeignKey(x => x.DepartmentId);
            builder.HasOne(x => x.DeletedBy).WithMany(x => x.DeletedUsers).HasForeignKey(x => x.DeletedById);

            builder.HasMany(x => x.CreatedStatements).WithOne(x => x.CreatedBy).HasForeignKey(x => x.CreatedById);
            builder.HasMany(x => x.CreatedEvidences).WithOne(x => x.CreatedBy).HasForeignKey(x => x.CreatedById);
            builder.HasMany(x => x.CreatedCases).WithOne(x => x.CreatedBy).HasForeignKey(x => x.CreatedById);
            builder.HasMany(x => x.DeletedCases).WithOne(x => x.DeletedBy).HasForeignKey(x => x.DeletedById);
            builder.HasMany(x => x.CaseUsers).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            builder.HasMany(x => x.CreatedReports).WithOne(x => x.CreatedBy).HasForeignKey(x => x.CreatedById);
            builder.HasMany(x => x.DeletedReports).WithOne(x => x.DeletedBy).HasForeignKey(x => x.DeletedById);
            builder.HasMany(x => x.LastEditedReports).WithOne(x => x.LastEditedBy).HasForeignKey(x => x.LastEditedById);
            builder.HasMany(x => x.LastEditedCases).WithOne(x => x.LastEditedBy).HasForeignKey(x => x.LastEditedById);
            builder.HasMany(x => x.DeletedCrimeScenes).WithOne(x => x.DeletedBy).HasForeignKey(x => x.DeletedById);
            builder.HasMany(x => x.CreatedDocuments).WithOne(x => x.CreatedBy).HasForeignKey(x => x.CreatedById);
            builder.HasMany(x => x.LastEditedDocuments).WithOne(x => x.LastEditedBy).HasForeignKey(x => x.LastEditedById);
            builder.HasMany(x => x.DeletedDocuments).WithOne(x => x.DeletedBy).HasForeignKey(x => x.DeletedById);
            builder.HasMany(x => x.LastEditedEvidences).WithOne(x => x.LastEditedBy).HasForeignKey(x => x.LastEditedById);
            builder.HasMany(x => x.DeletedEvidences).WithOne(x => x.DeletedBy).HasForeignKey(x => x.DeletedById);
            builder.HasMany(x => x.DeletedPersons).WithOne(x => x.DeletedBy).HasForeignKey(x => x.DeletedById);
            builder.HasMany(x => x.DeletedUsers).WithOne(x => x.DeletedBy).HasForeignKey(x => x.DeletedById);
        }
    }
}
