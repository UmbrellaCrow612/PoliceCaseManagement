using Microsoft.EntityFrameworkCore;
using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Core.Interfaces;

namespace PoliceCaseManagement.Infrastructure.Data.Repositories
{
    public class IncidentRepository(ApplicationDbContext context) : IIncidentRepository<Incident, string>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task AddAsync(Incident entity)
        {
            await _context.Incidents.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public Task<bool> DeleteAsync(string id, string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var incidentToDelete = await _context.Incidents.FirstOrDefaultAsync(x => x.Id == id);
            if(incidentToDelete is null) return false;

            _context.Incidents.Remove(incidentToDelete);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(string id)
        {
            var incidentExists = await _context.Incidents.AnyAsync(x => x.Id == id);

            return incidentExists;
        }

        public async Task<Incident?> GetByIdAsync(string id)
        {
            var incidentToGet = await _context.Incidents.FirstOrDefaultAsync(x => x.Id == id);

            return incidentToGet;
        }

        public async Task UpdateAsync(Incident updatedEntity)
        {
            _context.Incidents.Update(updatedEntity);
            await _context.SaveChangesAsync();
        }
    }
}
