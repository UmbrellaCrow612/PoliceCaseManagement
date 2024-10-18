using Microsoft.EntityFrameworkCore;
using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Core.Interfaces;

namespace PoliceCaseManagement.Infrastructure.Data.Repositories
{
    public class DepartmentRepository(ApplicationDbContext context) : IDepartmentRepository<Department, string>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task AddAsync(Department entity)
        {
            await _context.Departments.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var departmentToDelete = await _context.Departments.FirstOrDefaultAsync(x => x.Id == id);
            if (departmentToDelete is null) return false;

            _context.Departments.Remove(departmentToDelete);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Exists(string id)
        {
            var departmentExists = await _context.Departments.AnyAsync(x => x.Id == id);

            return departmentExists;
        }

        public async Task<Department?> GetByIdAsync(string id)
        {
            var departmentToGet = await _context.Departments.FirstOrDefaultAsync(x => x.Id == id);

            return departmentToGet is null ? null : departmentToGet;
        }

        public async Task UpdateAsync(Department updatedEntity)
        {
            _context.Departments.Update(updatedEntity);
            await _context.SaveChangesAsync();
        }
    }
}
