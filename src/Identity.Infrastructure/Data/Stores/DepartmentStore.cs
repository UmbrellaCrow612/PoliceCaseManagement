using Identity.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.Utils;

namespace Identity.Infrastructure.Data.Stores
{
    public class DepartmentStore(IdentityApplicationDbContext dbContext) : IDepartmentStore
    {
        private readonly IdentityApplicationDbContext _dbContext = dbContext;

        public IQueryable<Department> Departments => throw new NotImplementedException();

        public async Task AddUser(Department department, ApplicationUser user)
        {
            _ = _dbContext.Users.Local.FirstOrDefault(x => x.Id == user.Id) ?? throw new ApplicationException("User not being tracked");
            _ = _dbContext.Departments.Local.FirstOrDefault(x => x.Id == department.Id) ?? throw new ApplicationException("Department not being tracked");

            user.DepartmentId = department.Id;
            _dbContext.Users.Update(user);

            await _dbContext.SaveChangesAsync();
        }

        public async Task AddUsers(Department department, IEnumerable<ApplicationUser> users)
        {
            _ = _dbContext.Departments.Local.FirstOrDefault(x => x.Id == department.Id) ?? throw new ApplicationException("Department not being tracked");

            foreach (var user in users)
            {
                _ = _dbContext.Users.Local.FirstOrDefault(x => x.Id == user.Id) ?? throw new ApplicationException("User not being tracked");

                user.DepartmentId = department.Id;
            }

            _dbContext.Users.UpdateRange(users);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<(bool successful, ICollection<ErrorDetail> errors)> CreateDepartment(Department department)
        {
            List<ErrorDetail> errors = [];

            var nameAlreadyUsed = await _dbContext.Departments.AnyAsync(x => x.Name == department.Name);
            if(nameAlreadyUsed)
            {
                errors.Add(new ErrorDetail
                {
                    Field = "Departments",
                    Reason = "There is already a department with this name."
                });
                return (false, errors);
            }

            await _dbContext.Departments.AddAsync(department);
            await _dbContext.SaveChangesAsync();

            return (true, errors);
        }

        public async Task DeleteDepartment(Department department)
        {
            _ = _dbContext.Departments.Local.FirstOrDefault(x => x.Id == department.Id) ?? throw new ApplicationException("Department not being tracked");

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

        public async Task<bool> IsUserInDepartment(Department department, ApplicationUser user)
        {
            return await _dbContext.Users.AnyAsync(x => x.DepartmentId == department.Id && x.Id == user.Id);
        }

        public async Task RemoveUser(Department department, ApplicationUser user)
        {
            _ = _dbContext.Users.Local.FirstOrDefault(x => x.Id == user.Id) ?? throw new ApplicationException("User not being tracked");
            _ = _dbContext.Departments.Local.FirstOrDefault(x => x.Id == department.Id) ?? throw new ApplicationException("Department not being tracked");

            user.DepartmentId = null;
            _dbContext.Users.Update(user);

            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveUsers(Department department, IEnumerable<ApplicationUser> users)
        {
            _ = _dbContext.Departments.Local.FirstOrDefault(x => x.Id == department.Id) ?? throw new ApplicationException("Department not being tracked");

            foreach (var user in users)
            {
                _ = _dbContext.Users.Local.FirstOrDefault(x => x.Id == user.Id) ?? throw new ApplicationException("User not being tracked");

                user.DepartmentId = null;
            }

            _dbContext.Users.UpdateRange(users);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SetDepartment(Department department)
        {
            await _dbContext.Departments.AddAsync(department);
        }

        public async Task UpdateDepartment(Department department)
        {
            _ = _dbContext.Departments.Local.FirstOrDefault(x => x.Id == department.Id) ?? throw new ApplicationException("Department not being tracked");

            _dbContext.Departments.Update(department);
            await _dbContext.SaveChangesAsync();
        }
    }
}
