using Identity.Core.Models;
using Identity.Core.Services;
using Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.Implementations
{
    /// <summary>
    /// Business implementation of the contract <see cref="IRoleService"/> - test this, as well when using it else where only use the <see cref="IRoleService"/>
    /// interface not this class
    /// </summary>
    public class RoleServiceImpl(IdentityApplicationDbContext dbContext) : IRoleService
    {
        private readonly IdentityApplicationDbContext _dbContext = dbContext;

        public async Task<List<ApplicationRole>> GetRolesAsync(ApplicationUser use)
        {
            var roleIds = await _dbContext.UserRoles
                .Where(x => x.UserId == user.Id)
                .Select(x => x.RoleId)
                .ToListAsync();

            return await _dbContext.Roles
                .Where(r => roleIds.Contains(r.Id))
                .ToListAsync();
        }
    }
}
