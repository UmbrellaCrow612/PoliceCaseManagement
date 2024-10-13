using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasOne(x => x.Case).WithMany(x => x.Reports).HasForeignKey(x => x.CaseId);
            builder.HasOne(x => x.CreatedBy).WithMany(x => x.CreatedReports).HasForeignKey(x => x.CreatedById);
            builder.HasOne(x => x.DeletedBy).WithMany(x => x.DeletedReports).HasForeignKey(x => x.DeletedById);
            builder.HasOne(x => x.LastEditedBy).WithMany(x => x.LastEditedReports).HasForeignKey(x => x.LastEditedById);
        }
    }
}
