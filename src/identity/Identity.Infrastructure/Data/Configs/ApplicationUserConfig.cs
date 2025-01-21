using Identity.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Data.Configs
{
    internal class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasIndex(x => x.PhoneNumber).IsUnique();

            builder.HasOne(x => x.TimeBasedOneTimePassCode).WithOne(x => x.User).HasForeignKey<TimeBasedOneTimePassCode>(e => e.UserId);
        }
    }
}
