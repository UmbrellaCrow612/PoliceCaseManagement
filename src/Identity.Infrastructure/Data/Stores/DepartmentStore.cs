using Identity.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data.Stores
{
    public class DepartmentStore(IdentityApplicationDbContext dbContext) : IDepartmentStore
    {
        private readonly IdentityApplicationDbContext _dbContext = dbContext;

        public async Task AddUser(Department department, ApplicationUser user)
        {
            user.DepartmentId = department.Id;
            _dbContext.Users.Update(user);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteDepartment(Department department)
        {
            _dbContext.Departments.Remove(department);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Department?> GetDepartmentById(string id)
        {
            return await _dbContext.Departments.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsers(Department department)
        {
            return await _dbContext.Users.Where(x => x.DepartmentId == department.Id).ToListAsync();
        }

        public async Task RemoveUser(Department department, ApplicationUser user)
        {
            user.DepartmentId = null;
            _dbContext.Users.Update(user);

            await _dbContext.SaveChangesAsync();
        }

        public async Task SetDepartment(Department department)
        {
            await _dbContext.Departments.AddAsync(department);
        }

        public async Task StoreDepartment(Department department)
        {
            await _dbContext.Departments.AddAsync(department);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateDepartment(Department department)
        {
            _dbContext.Departments.Update(department);
            await _dbContext.SaveChangesAsync();
        }
    }
}
