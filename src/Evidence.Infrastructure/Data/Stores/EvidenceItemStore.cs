using Evidence.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Evidence.Infrastructure.Data.Stores
{
    public class EvidenceItemStore(EvidenceApplicationDbContext dbContext) : IEvidenceItemStore
    {
        private readonly EvidenceApplicationDbContext _dbContext = dbContext;

        public IQueryable<EvidenceItem> Evidences => _dbContext.Evidences.AsQueryable();

        public async Task<(bool Succeeded, IEnumerable<string> Errors)> CreateEvidence(EvidenceItem evidence)
        {
            IEnumerable<string> errors = [];

            await _dbContext.Evidences.AddAsync(evidence);
            await _dbContext.SaveChangesAsync();

            return (true, errors);
        }

        public async Task DeleteEvidence(string userId, EvidenceItem evidence)
        {
            evidence.IsDeleted = true;
            evidence.DeletedAt = DateTime.UtcNow;
            evidence.DeletedById = userId;

            _dbContext.Evidences.Update(evidence);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<EvidenceItem?> GetEvidenceById(string id)
        {
            return await _dbContext.Evidences.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task UpdateEvidence(string userId, EvidenceItem evidence)
        {
            evidence.LastEditedAt = DateTime.UtcNow;
            evidence.LastEditedById = userId;

            _dbContext.Evidences.Update(evidence);
            await _dbContext.SaveChangesAsync();
        }
    }
}
