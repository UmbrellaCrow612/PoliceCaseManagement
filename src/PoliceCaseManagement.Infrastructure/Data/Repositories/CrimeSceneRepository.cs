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

        public async Task<bool> DeleteAsync(CrimeScene entity)
        {
            var crimeSceneToDelete = await _context.CrimeScenes.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (crimeSceneToDelete is null) return false;

            _context.CrimeScenes.Remove(crimeSceneToDelete);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<CrimeScene?> GetByIdAsync(string id)
        {
            var crimeSceneToGet = await _context.CrimeScenes.FirstOrDefaultAsync(x => x.Id == id);
            if (crimeSceneToGet is null) return null;

            return crimeSceneToGet;
        }

        public Task<IEnumerable<CrimeScene>> SearchAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(CrimeScene entity)
        {
            var crimeSceneToUpdate = await _context.CrimeScenes.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (crimeSceneToUpdate is null) return false;

            _context.Entry(crimeSceneToUpdate).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
