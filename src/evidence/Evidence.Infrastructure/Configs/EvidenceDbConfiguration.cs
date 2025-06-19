using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evidence.Infrastructure.Configs
{
    /// <summary>
    /// How the <see cref="Core.Models.Evidence"/> table will be configured in the database table
    /// </summary>
    internal class EvidenceDbConfiguration : IEntityTypeConfiguration<Core.Models.Evidence>
    {
        public void Configure(EntityTypeBuilder<Core.Models.Evidence> builder)
        {
            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasIndex(x => x.ReferenceNumber).IsUnique();
        }
    }
}
