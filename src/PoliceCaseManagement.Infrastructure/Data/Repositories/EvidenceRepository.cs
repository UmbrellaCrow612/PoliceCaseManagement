using Microsoft.EntityFrameworkCore;
using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Core.Interfaces;

namespace PoliceCaseManagement.Infrastructure.Data.Repositories
{
    public class EvidenceRepository(ApplicationDbContext context) : IEvidenceRepository<Evidence, string>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task AddAsync(Evidence entity)
        {
            await _context.Evidences.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(string id, string userId)
        {
            var evidenceToDelete = await _context.Evidences.FirstOrDefaultAsync(x => x.Id == id);
            if (evidenceToDelete is null) return false;

            evidenceToDelete.IsDeleted = true;
            evidenceToDelete.DeletedAt = DateTime.UtcNow;
            evidenceToDelete.DeletedById = userId;

            await _context.SaveChangesAsync();
            return true;
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(string id)
        {
            var evidenceExists = await _context.Evidences.AnyAsync(x => x.Id == id);

            return evidenceExists;
        }

        public async Task<Evidence?> GetByIdAsync(string id)
        {
            var evidenceToGet = await _context.Evidences.FirstOrDefaultAsync(x => x.Id == id);

            return evidenceToGet;
        }

        public async Task UpdateAsync(Evidence updatedEntity)
        {
            _context.Evidences.Update(updatedEntity);
            await _context.SaveChangesAsync();

        }
    }
}
