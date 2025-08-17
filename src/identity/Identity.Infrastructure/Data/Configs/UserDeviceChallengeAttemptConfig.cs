using Identity.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Data.Configs
{
    internal class UserDeviceChallengeAttemptConfig : IEntityTypeConfiguration<DeviceVerification>
    {
        public void Configure(EntityTypeBuilder<DeviceVerification> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}
