using Evidence.Infrastructure.Data.Models;

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

        public async Task<(bool Succeeded, ICollection<string> Errors)> DeleteCrimeScene(CrimeScene crimeScene)
        {
            List<string> errors = [];

            var crimeSceneInContext = _dbContext.CrimeScenes.Local.FirstOrDefault(x => x.Id == crimeScene.Id);
            if (crimeSceneInContext is null) errors.Add("CrimeScene is not in context.");

            if (errors.Count != 0) return (false, errors);

            _dbContext.CrimeScenes.Remove(crimeScene);
            await _dbContext.SaveChangesAsync();

            return (true, errors);
        }

        public async Task<CrimeScene?> GetCrimeSceneById(string crimeSceneId)
        {
            
            return await _dbContext.CrimeScenes.FindAsync(crimeSceneId);
        }

        public async Task<(bool Succeeded, ICollection<string> Errors)> UpdateCrimeScene(CrimeScene crimeScene)
        {
            List<string> errors = [];

            var crimeSceneInContext = _dbContext.CrimeScenes.Local.FirstOrDefault(x => x.Id == crimeScene.Id);
            if (crimeSceneInContext is null) errors.Add("CrimeScene is not in context.");

            if (errors.Count != 0) return (false, errors);

            _dbContext.CrimeScenes.Update(crimeScene);
            await _dbContext.SaveChangesAsync();

            return (true, errors);
        }
    }
}
