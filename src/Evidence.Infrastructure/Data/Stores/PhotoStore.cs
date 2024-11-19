using Evidence.Infrastructure.Data.Models;

namespace Evidence.Infrastructure.Data.Stores
{
    public class PhotoStore : IPhotoStore
    {
        public Task<(bool Succeeded, IEnumerable<string> Errors)> CreatePhoto(EvidenceItem evidence, Photo photo)
        {
            throw new NotImplementedException();
        }

        public Task DeletePhoto(EvidenceItem evidence, Photo photo)
        {
            throw new NotImplementedException();
        }

        public Task<Photo> GetPhotoById(EvidenceItem evidence, string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Photo>> GetPhotos(EvidenceItem evidence)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePhoto(EvidenceItem evidence, Photo photo)
        {
            throw new NotImplementedException();
        }
    }
}
