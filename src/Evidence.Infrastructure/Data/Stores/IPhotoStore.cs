using Evidence.Infrastructure.Data.Models;

namespace Evidence.Infrastructure.Data.Stores
{
    public interface IPhotoStore
    {
        Task<(bool Succeeded, IEnumerable<string> Errors)> CreatePhoto(EvidenceItem evidence, Photo photo);
        Task<IEnumerable<Photo>> GetPhotos(EvidenceItem evidence);
        Task<Photo> GetPhotoById(EvidenceItem evidence, string id);
        Task UpdatePhoto(EvidenceItem evidence, Photo photo);
        Task DeletePhoto(EvidenceItem evidence, Photo photo);
    }
}
