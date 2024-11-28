using Evidence.Infrastructure.Data.Models;

namespace Evidence.Infrastructure.Data.Stores
{
    public interface IPhotoStore
    {
        IQueryable<Photo> Photos { get; }

        Task<(bool Succeeded, ICollection<string> Errors)> CreatePhotoAsync(Photo photo);
        Task<Photo?> GetPhotoByIdAsync(string photoId);
        Task<bool> IsPhotoInEvidenceAsync(string photoId, EvidenceItem evidence);
        Task<bool> IsPhotoInCrimeSceneAsync(string photoId, CrimeScene crimeScene);
        Task<ICollection<EvidenceItem>> GetEvidencesAsync(Photo photo);
        Task<ICollection<CrimeScene>> GetCrimeScenesAsync(Photo photo);
        Task<(bool Succeeded, ICollection<string> Errors)> UpdatePhotoAsync(Photo photo);
        Task<(bool Succeeded, ICollection<string> Errors)> DeletePhotoAsync(Photo photo);
    }
}
