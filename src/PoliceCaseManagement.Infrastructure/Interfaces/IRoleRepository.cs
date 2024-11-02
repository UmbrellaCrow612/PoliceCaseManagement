using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Infrastructure.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<bool> RoleNameExistsAsync (string roleName);
        /// <summary>
        /// Check if a user is already linked to a role.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="roleName">The name of the role</param>
        /// <returns> <see langword="true"/> if they are linked or <see langword="false"/> if they are not </returns>
        Task<bool> UserLinkedToRoleAsync (string userId, string roleName);
        Task LinkUserToRoleAsync (string userId, string roleName);
    }
}
