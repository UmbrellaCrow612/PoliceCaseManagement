using Identity.Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data
{
    public class IdentityApplicationDbContext(DbContextOptions<IdentityApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
    }
}
