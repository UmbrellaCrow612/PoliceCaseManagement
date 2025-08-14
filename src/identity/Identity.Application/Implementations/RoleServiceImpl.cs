using Identity.Core.Models;
using Identity.Core.Services;

namespace Identity.Application.Implementations
{
    /// <summary>
    /// Business implementation of the contract <see cref="IRoleService"/> - test this, as well when using it else where only use the <see cref="IRoleService"/>
    /// interface not this class
    /// </summary>
    public class RoleServiceImpl : IRoleService
    {
        public Task<List<ApplicationRole>> GetRolesAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}
