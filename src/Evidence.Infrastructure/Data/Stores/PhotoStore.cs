using Evidence.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Utils;

namespace Evidence.Infrastructure.Data.Stores
{
    public class PhotoStore(EvidenceApplicationDbContext dbContext) : IPhotoStore
    {
        private readonly EvidenceApplicationDbContext _dbContext = dbContext;

        public IQueryable<Photo> Photos => _dbContext.Photos.AsQueryable();

        public async Task<(bool Succeeded, ICollection<string> Errors)> CreatePhotoAsync(Photo photo)
        {
            List<string> errors = [];

            if (!Uri.IsWellFormedUriString(photo.FilePathUrl, UriKind.Absolute)) errors.Add("File path uri is not well formatted.");

            if (errors.Count != 0) return (false, errors);

            await _dbContext.Photos.AddAsync(photo);
            await _dbContext.SaveChangesAsync();

            return (true, []);
        }

        public async Task<(bool Succeeded, ICollection<string> Errors)> DeletePhotoAsync(Photo photo)
        {
            List<string> errors = [];

            var exists = EfHelper.ExistsInContext(_dbContext, photo);

            if(!exists) errors.Add("Photo not in context.");

            var isLinkedToEvidence = await _dbContext.EvidenceItemPhotos.AnyAsync(x => x.PhotoId == photo.Id);
            if (isLinkedToEvidence) errors.Add("Photo is linked to evidence");

            var isLinkedToCrimeScene = await _dbContext.CrimeScenePhotos.AnyAsync(x => x.PhotoId == photo.Id);
            if (isLinkedToCrimeScene) errors.Add("Photo is linked to at least one crime scene.");

            if (errors.Count != 0) return (false, errors);

            _dbContext.Photos.Remove(photo);
            await _dbContext.SaveChangesAsync();

            return (true, errors);
        }

        public async Task<ICollection<CrimeScene>> GetCrimeScenesAsync(Photo photo)
        {
            return await _dbContext.CrimeScenePhotos
                .Where(x => x.PhotoId == photo.Id)
                .Include(x => x.CrimeScene)
                .Select(x => x.CrimeScene)
                .ToListAsync();
        }

        public async Task<ICollection<EvidenceItem>> GetEvidencesAsync(Photo photo)
        {
            return await _dbContext.EvidenceItemPhotos
                .Where(x => x.PhotoId == photo.Id)
                .Include(x => x.Evidence)
                .Select(x => x.Evidence)
                .ToListAsync();

        }

        public async Task<Photo?> GetPhotoByIdAsync(string photoId)
        {
            return await _dbContext.Photos.FirstOrDefaultAsync(x => x.Id == photoId);
        }

        public async Task<bool> IsPhotoInCrimeSceneAsync(string photoId, CrimeScene crimeScene)
        {
            return await _dbContext.CrimeScenePhotos.AnyAsync(x => x.PhotoId == photoId && x.CrimeSceneId == crimeScene.Id);
        }

        public async Task<bool> IsPhotoInEvidenceAsync(string photoId, EvidenceItem evidence)
        {
            return await _dbContext.EvidenceItemPhotos.AnyAsync(x => x.PhotoId == photoId && x.EvidenceItemId == evidence.Id);
        }

        public async Task<(bool Succeeded, ICollection<string> Errors)> UpdatePhotoAsync(Photo photo)
        {
            List<string> errors = [];

            if (!Uri.IsWellFormedUriString(photo.FilePathUrl, UriKind.Absolute)) errors.Add("File path uri is not well formatted.");

            var exists = EfHelper.ExistsInContext(_dbContext, photo);
            if (!exists) errors.Add("Photo not in context.");

            if(errors.Count != 0) return (false, errors);

            _dbContext.Photos.Update(photo);
            await _dbContext.SaveChangesAsync();

            return (true, errors);
        }
    }
}
