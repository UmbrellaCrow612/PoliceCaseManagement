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

        public async Task<bool> DeleteAsync(Case entity)
        {
            var caseToDelete = await _context.Cases.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (caseToDelete is null) return false;

            _context.Cases.Remove(caseToDelete);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Case?> GetByIdAsync(string id)
        {
            var caseToGet = await _context.Cases.FirstOrDefaultAsync(x => x.Id == id);
            if (caseToGet is null) return null;

            return caseToGet;
        }
        public Task<IEnumerable<Case>> SearchAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(Case entity)
        {
            var caseToUpdate = await _context.Cases.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (caseToUpdate is null) return false;

            _context.Entry(caseToUpdate).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
