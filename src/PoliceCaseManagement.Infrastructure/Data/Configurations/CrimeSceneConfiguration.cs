using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Infrastructure.Data.Configurations
{
    public class CrimeSceneConfiguration : IEntityTypeConfiguration<CrimeScene>
    {
        public void Configure(EntityTypeBuilder<CrimeScene> builder)
        {
            throw new NotImplementedException();
        }
    }
}
