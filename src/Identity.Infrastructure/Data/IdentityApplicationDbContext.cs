using Identity.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data
{
    public class IdentityApplicationDbContext(DbContextOptions<IdentityApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Token> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
