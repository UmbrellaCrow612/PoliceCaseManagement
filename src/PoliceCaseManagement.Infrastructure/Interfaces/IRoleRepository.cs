using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Infrastructure.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<bool> RoleNamesExistsAsync(IEnumerable<string> roles);
        Task<bool> RoleNameExistsAsync (string roleName);
        Task<bool> UserLinkedToRoleAsync (string userId, string roleName);
        Task LinkUserToRolesAsync(string userId, IEnumerable<string> roles);
        Task LinkUserToRoleAsync (string userId, string roleName);
        Task UnLinkUserFromRoleAsync(string userId, string roleName);
        Task<string> GetRoleId(string roleName);
    }
}
