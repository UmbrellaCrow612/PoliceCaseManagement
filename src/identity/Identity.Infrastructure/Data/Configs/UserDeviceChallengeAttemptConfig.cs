using Identity.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Data.Configs
{
    internal class UserDeviceChallengeAttemptConfig : IEntityTypeConfiguration<UserDeviceChallengeAttempt>
    {
        public void Configure(EntityTypeBuilder<UserDeviceChallengeAttempt> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasIndex(x => x.Code).IsUnique();
        }
    }
}
