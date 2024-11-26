using Evidence.Infrastructure.Data.Models;

namespace Evidence.Infrastructure.Data.Stores
{
    public class CrimeSceneStore : ICrimeSceneStore
    {
        public IQueryable<CrimeScene> CrimeScenes => throw new NotImplementedException();

        public Task<(bool Succeeded, ICollection<string> Errors)> CreateCrimeScene(CrimeScene crimeScene)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCrimeScene(CrimeScene crimeScene)
        {
            throw new NotImplementedException();
        }

        public Task<CrimeScene?> GetCrimeSceneById(string crimeSceneId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCrimeScene(CrimeScene crimeScene)
        {
            throw new NotImplementedException();
        }
    }
}
