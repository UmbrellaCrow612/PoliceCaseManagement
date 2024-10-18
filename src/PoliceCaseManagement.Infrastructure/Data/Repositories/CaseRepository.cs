using Microsoft.EntityFrameworkCore;
using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Core.Interfaces;

namespace PoliceCaseManagement.Infrastructure.Data.Repositories
{
    public class CaseRepository(ApplicationDbContext context) : ICaseRepository<Case, string>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task AddAsync(Case entity)
        {
            await _context.Cases.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var caseToDelete = await _context.Cases.FirstOrDefaultAsync(c => c.Id == id);
            if (caseToDelete is null) return false;

            _context.Cases.Remove(caseToDelete);
            return true;
        }

        public async Task<bool> Exists(string id)
        {
            var caseExists = await _context.Cases.AnyAsync(x => x.Id == id);

            return caseExists;
        }

        public async Task<Case?> GetByIdAsync(string id)
        {
            var caseToGet = await _context.Cases.FirstOrDefaultAsync(x => x.Id == id);

            return caseToGet is null ? null : caseToGet;
        }

        public async Task UpdateAsync(Case updatedEntity)
        {
            _context.Cases.Update(updatedEntity);
            await _context.SaveChangesAsync();
        }
    }
}
