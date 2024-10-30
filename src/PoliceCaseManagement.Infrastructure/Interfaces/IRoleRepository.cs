using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Infrastructure.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<bool> RoleNameExistsAsync (string roleName);
        Task<bool> UserLinkedToRoleAsync (string userId, string roleName);
        Task LinkUserToRoleAsync (string userId, string roleName);
    }
}
