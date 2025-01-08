using CAPTCHA.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CAPTCHA.Infrastructure.Data.Configs
{
    internal class CAPTCHAGridConfig : IEntityTypeConfiguration<CAPTCHAGridQuestion>
    {
        public void Configure(EntityTypeBuilder<CAPTCHAGridQuestion> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id);

            builder.HasMany(x => x.Children).WithOne(x => x.Question).HasForeignKey(x => x.CAPTCHAGridParentQuestionId);
        }
    }
}
