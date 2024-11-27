using Evidence.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Evidence.Infrastructure.Data.Stores
{
    public class CrimeSceneStore(EvidenceApplicationDbContext dbContext) : ICrimeSceneStore
    {
        private readonly EvidenceApplicationDbContext _dbContext = dbContext;

        public IQueryable<CrimeScene> CrimeScenes => _dbContext.CrimeScenes.AsQueryable();

        public async Task<(bool Succeeded, ICollection<string> Errors)> CreateCrimeScene(CrimeScene crimeScene)
        {
            await _dbContext.CrimeScenes.AddAsync(crimeScene);
            await _dbContext.SaveChangesAsync();

            return (true, []);
        }

        public async Task DeleteCrimeScene(CrimeScene crimeScene)
        {
            _ = _dbContext.CrimeScenes.Local.FirstOrDefault(x => x.Id == crimeScene.Id) ?? throw new ApplicationException("Crime scene not in context.");

            _dbContext.CrimeScenes.Remove(crimeScene);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CrimeScene?> GetCrimeSceneById(string crimeSceneId)
        {
            return await _dbContext.CrimeScenes.FirstOrDefaultAsync(x => x.Id == crimeSceneId);
        }

        public async Task UpdateCrimeScene(CrimeScene crimeScene)
        {
            _ = _dbContext.CrimeScenes.Local.FirstOrDefault(x => x.Id == crimeScene.Id) ?? throw new ApplicationException("Crime scene not in context.");

            _dbContext.CrimeScenes.Update(crimeScene);
            await _dbContext.SaveChangesAsync();
        }
    }
}
