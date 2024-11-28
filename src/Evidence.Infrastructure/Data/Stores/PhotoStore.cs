using Evidence.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Evidence.Infrastructure.Data.Stores
{
    public class PhotoStore(EvidenceApplicationDbContext dbContext) : IPhotoStore
    {
        private readonly EvidenceApplicationDbContext _dbContext = dbContext;

        public Task<IQueryable<Photo>> Photos => Task.FromResult(_dbContext.Photos.AsQueryable());

        public async Task<(bool Succeeded, ICollection<string> Errors)> CreatePhotoAsync(Photo photo)
        {
            await _dbContext.Photos.AddAsync(photo);
            await _dbContext.SaveChangesAsync();

            return (true, []);
        }

        public async Task<(bool Succeeded, ICollection<string> Errors)> DeletePhotoAsync(Photo photo)
        {
            List<string> Errors = [];

            // check if it was queryed from db and in context
            var inContext = _dbContext.Photos.Local.FirstOrDefault(x => x.Id == photo.Id);
            if(inContext is null)
            {
                Errors.Add("Photo not found.");

                return (false, Errors);
            }

            // linked to evidence 
            var linkedToEvidence = await _dbContext.EvidenceItemPhotos.AnyAsync(x => x.PhotoId == photo.Id);
            if (linkedToEvidence) Errors.Add("Photo is linked to a Evidence");

            // linked to crime scene 
            var linkedToCrimeScene = await _dbContext.CrimeScenePhotos.AnyAsync(x => x.PhotoId == photo.Id);
            if (linkedToCrimeScene) Errors.Add("Photo is linked to a Crime Scene.");


            _dbContext.Photos.Remove(photo);

            await _dbContext.SaveChangesAsync();

            return (true, Errors);
        }

        public Task<ICollection<CrimeScene>> GetCrimeScenesAsync(Photo photo)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<EvidenceItem>> GetEvidencesAsync(Photo photo)
        {
            throw new NotImplementedException();
        }

        public Task<Photo> GetPhotoByIdAsync(string photoId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsPhotoInCrimeSceneAsync(string photoId, CrimeScene crimeScene)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsPhotoInEvidenceAsync(string photoId, EvidenceItem evidence)
        {
            throw new NotImplementedException();
        }

        public Task<(bool Succeeded, ICollection<string> Errors)> UpdatePhotoAsync(Photo photo)
        {
            throw new NotImplementedException();
        }
    }
}
