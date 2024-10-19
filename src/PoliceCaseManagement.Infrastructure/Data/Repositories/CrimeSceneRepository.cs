using Microsoft.EntityFrameworkCore;
using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Core.Interfaces;

namespace PoliceCaseManagement.Infrastructure.Data.Repositories
{
    public class CrimeSceneRepository(ApplicationDbContext context) : ICrimeSceneRepository<CrimeScene, string>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task AddAsync(CrimeScene entity)
        {
            await _context.CrimeScenes.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(string id, string userId)
        {
            var crimeSceneToDelete = await _context.CrimeScenes.FirstOrDefaultAsync(x => x.Id == id);
            if (crimeSceneToDelete is null) return false;

            crimeSceneToDelete.IsDeleted = true;
            crimeSceneToDelete.DeletedAt = DateTime.UtcNow;
            crimeSceneToDelete.DeletedById = userId;

            await _context.SaveChangesAsync();
            return true;
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(string id)
        {
            var crimeSceneExists = await _context.CrimeScenes.AnyAsync(x => x.Id == id);

            return crimeSceneExists;
        }

        public async Task<CrimeScene?> GetByIdAsync(string id)
        {
            var crimeSceneToGet = await _context.CrimeScenes.FirstOrDefaultAsync(x => x.Id == id);

            return crimeSceneToGet is null ? null : crimeSceneToGet;
        }

        public async Task UpdateAsync(CrimeScene updatedEntity)
        {
             _context.CrimeScenes.Update(updatedEntity);
            await _context.SaveChangesAsync();
        }
    }
}
