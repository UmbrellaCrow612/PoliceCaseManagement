using Identity.Core.Models;
using Results.Abstractions;

namespace Identity.Core.Services
{
    /// <summary>
    /// Provides role functionality 
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// Get all the roles for a given user
        /// </summary>
        /// <param name="user">The user to get the roles for</param>
        Task<List<ApplicationRole>> GetRolesAsync(ApplicationUser user);

        /// <summary>
        /// Create a role
        /// </summary>
        /// <param name="role">The role to create </param>
        Task<IResult> CreateAsync(ApplicationRole role);
    }
}
