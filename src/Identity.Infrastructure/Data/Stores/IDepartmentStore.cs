using Identity.Infrastructure.Data.Models;

namespace Identity.Infrastructure.Data.Stores
{
    public interface IDepartmentStore
    {
        Task SetDepartment(Department department);
        Task StoreDepartment(Department department);
        Task<Department?> GetDepartmentById(string id);
        Task UpdateDepartment(Department department);
        Task DeleteDepartment(Department department);
        Task<bool> IsUserInDepartment(Department department, ApplicationUser user);
        Task AddUsers(Department department, IEnumerable<ApplicationUser> users);
        Task RemoveUsers(Department department, IEnumerable<ApplicationUser> users);
        Task AddUser(Department department, ApplicationUser user);
        Task RemoveUser(Department department, ApplicationUser user);
        Task<IEnumerable<ApplicationUser>> GetUsers(Department department);
    }
}
