using Evidence.Infrastructure.Data.Models;

namespace Evidence.Infrastructure.Data.Stores
{
    public interface ICrimeSceneStore
    {
        Task<IQueryable<CrimeScene>> CrimeScenes { get; }

        Task<(bool Succeeded, ICollection<string> Errors)> CreateCrimeScene(CrimeScene crimeScene);
        Task<CrimeScene?> GetCrimeSceneById(string crimeSceneId);
        Task UpdateCrimeScene(CrimeScene crimeScene);
        Task DeleteCrimeScene(CrimeScene crimeScene);
    }
}
