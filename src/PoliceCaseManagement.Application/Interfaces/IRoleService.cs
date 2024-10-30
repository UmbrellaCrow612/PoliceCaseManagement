using PoliceCaseManagement.Application.DTOs.Roles;

namespace PoliceCaseManagement.Application.Interfaces
{
    public interface IRoleService
    {
        Task AddRoleAsync (CreateRoleDto role);
        Task RoleNameExistsAsync (string roleName);

        Task LinkUserToRoleAsync(string roleName, string userId);

    }
}
