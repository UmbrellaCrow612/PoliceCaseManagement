using Evidence.Infrastructure.Data.Models;

namespace Evidence.Infrastructure.Data.Stores
{
    public class EvidenceItemStore(EvidenceApplicationDbContext dbContext) : IEvidenceItemStore
    {
        private readonly EvidenceApplicationDbContext _dbContext = dbContext;

        public IQueryable<EvidenceItem> Evidences => _dbContext.Evidences.AsQueryable();

        public async Task<(bool Succeeded, ICollection<string> Errors)> CreateEvidenceAsync(EvidenceItem evidence)
        {
            List<string> errors = [];

            if (evidence.CreatedById is null)
            {
                errors.Add("Created by ID is null");
                return (false, errors);
            }

            await _dbContext.Evidences.AddAsync(evidence);
            await _dbContext.SaveChangesAsync();

            return (true, errors);
        }

        public async Task<(bool Succeeded, ICollection<string> Errors)> DeleteEvidenceAsync(string userId, EvidenceItem evidence)
        {
            List<string> errors = [];

            var inContext = _dbContext.Evidences.Local.FirstOrDefault(x => x.Id == evidence.Id);
            if(inContext is null)
            {
                errors.Add("Evidence not in context.");
                return (false, errors);
            }

            if(evidence.DeletedById is not null || evidence.DeletedAt is not null || evidence.IsDeleted is true)
            {
                errors.Add("Evidence is already marked deleted.");
                return (false, errors);
            }

            evidence.IsDeleted = true;
            evidence.DeletedById = userId;
            evidence.DeletedAt = DateTime.UtcNow;

            _dbContext.Evidences.Update(evidence);
            await _dbContext.SaveChangesAsync();

            return (true, errors);
        }

        public async Task<EvidenceItem?> GetEvidenceByIdAsync(string evidenceId)
        {
            return await _dbContext.Evidences.FindAsync(evidenceId);
        }

        public async Task<(bool Succeeded, ICollection<string> Errors)> UpdateEvidenceAsync(string userId, EvidenceItem evidence)
        {
            List<string> errors = [];

            var inContext = _dbContext.Evidences.Local.FirstOrDefault(x => x.Id == evidence.Id);
            if (inContext is null)
            {
                errors.Add("Evidence not in context.");
                return (false, errors);
            }

            evidence.LastEditedById = userId;
            evidence.LastEditedAt = DateTime.UtcNow;

            _dbContext.Evidences.Update(evidence);
            await _dbContext.SaveChangesAsync();

            return (true, errors);
        }
    }
}
