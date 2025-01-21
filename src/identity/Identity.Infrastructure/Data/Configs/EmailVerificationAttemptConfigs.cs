using Identity.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Data.Configs
{
    internal class EmailVerificationAttemptConfigs : IEntityTypeConfiguration<EmailVerificationAttempt>
    {
        public void Configure(EntityTypeBuilder<EmailVerificationAttempt> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasIndex(x => x.Code).IsUnique();
        }
    }
}
