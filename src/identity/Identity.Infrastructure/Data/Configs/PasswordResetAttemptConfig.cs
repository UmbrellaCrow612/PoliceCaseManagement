using Identity.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Data.Configs
{
    internal class PasswordResetAttemptConfig : IEntityTypeConfiguration<PasswordResetAttempt>
    {
        public void Configure(EntityTypeBuilder<PasswordResetAttempt> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasIndex(x => x.Code).IsUnique();
        }
    }
}
