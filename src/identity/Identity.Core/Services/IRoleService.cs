using Identity.Core.Models;

namespace Identity.Core.Services
{
    public interface IRoleService
    {
        /// <summary>
        /// Get all the roles for a given user
        /// </summary>
        /// <param name="user">The user to get the roles for</param>
        Task<List<ApplicationRole>> GetRolesAsync(ApplicationUser user);
    }
}
