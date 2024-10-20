using Microsoft.EntityFrameworkCore;
using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Core.Interfaces;

namespace PoliceCaseManagement.Infrastructure.Data.Repositories
{
    public class LocationRepository(ApplicationDbContext context) : ILocationRepository<Location, string>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task AddAsync(Location entity)
        {
            await _context.Locations.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public Task<bool> DeleteAsync(string id, string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var locationToDelete = await _context.Locations.FirstOrDefaultAsync(x => x.Id == id);
            if (locationToDelete is null) return false;

            _context.Locations.Remove(locationToDelete);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(string id)
        {
            var locationExists = await _context.Locations.AnyAsync(x => x.Id == id);

            return locationExists;
        }

        public async Task<Location?> GetByIdAsync(string id)
        {
            var locationToGet = await _context.Locations.FirstOrDefaultAsync(x => x.Id == id);

            return locationToGet;
        }

        public async Task UpdateAsync(Location updatedEntity)
        {
            _context.Locations.Update(updatedEntity);
            await _context.SaveChangesAsync();
        }
    }
}
