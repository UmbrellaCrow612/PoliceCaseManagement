using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations
{
    public class CaseConfiguration : IEntityTypeConfiguration<Case>
    {
        public void Configure(EntityTypeBuilder<Case> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasIndex(x => x.CaseNumber).IsUnique();

            builder.Property(x => x.Title).HasMaxLength(120);
            builder.Property(x => x.Description).HasMaxLength(250);

            builder.HasMany(x => x.CasePersons).WithOne(x => x.Case).HasForeignKey(x => x.CaseId);
            builder.HasMany(x => x.CaseUsers).WithOne(x => x.Case).HasForeignKey(x => x.CaseId);
            builder.HasMany(x => x.CaseTags).WithOne(x => x.Case).HasForeignKey(x => x.CaseId);
            builder.HasMany(x => x.CaseEvidences).WithOne(x => x.Case).HasForeignKey(x => x.CaseId);
            builder.HasMany(x => x.CaseCrimeScenes).WithOne(x => x.Case).HasForeignKey(x => x.CaseId);
            builder.HasMany(x => x.Reports).WithOne(x => x.Case).HasForeignKey(x => x.CaseId);
            builder.HasMany(x => x.CaseDocuments).WithOne(x => x.Case).HasForeignKey(x => x.CaseId);
            builder.HasOne(x => x.Department).WithMany(x => x.AssignedCases).HasForeignKey(x => x.DepartmentId);
            builder.HasOne(x => x.CreatedBy).WithMany(x => x.CreatedCases).HasForeignKey(x => x.CreatedById);
            builder.HasOne(x => x.DeletedBy).WithMany(x => x.DeletedCases).HasForeignKey(x => x.DeletedById);
            builder.HasOne(x => x.LastEditedBy).WithMany(x => x.LastEditedCases).HasForeignKey(x => x.LastEditedById);
        }
    }
}
