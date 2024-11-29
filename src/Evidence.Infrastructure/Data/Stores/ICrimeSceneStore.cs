using Evidence.Infrastructure.Data.Models;

namespace Evidence.Infrastructure.Data.Stores
{
    public interface ICrimeSceneStore
    {
        IQueryable<CrimeScene> CrimeScenes { get; }

        Task<(bool Succeeded, ICollection<string> Errors)> CreateCrimeScene(CrimeScene crimeScene);
        Task<CrimeScene?> GetCrimeSceneById(string crimeSceneId);
        Task<(bool Succeeded, ICollection<string> Errors)> UpdateCrimeScene(CrimeScene crimeScene);
        Task<(bool Succeeded, ICollection<string> Errors)> DeleteCrimeScene(CrimeScene crimeScene);
    }
}
