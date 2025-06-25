using Evidence.Core.Services;
using Evidence.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Evidence.Application.Implementations
{
    public class EvidenceService(EvidenceApplicationDbContext dbContext) : IEvidenceService
    {
        private readonly EvidenceApplicationDbContext _dbcontext = dbContext;

        public Task<EvidenceServiceResult> CreateAsync(Core.Models.Evidence evidence)
        {
            throw new NotImplementedException();
        }

        public Task<EvidenceServiceResult> DeleteAsync(Core.Models.Evidence evidence)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(string evidenceId)
        {
            return await _dbcontext.Evidences.AnyAsync(x => x.Id == evidenceId);
        }

        public async Task<Core.Models.Evidence?> FindByIdAsync(string evidenceId)
        {
            return await _dbcontext.Evidences.FindAsync(evidenceId);
        }

        public async Task<bool> IsReferenceNumberTaken(string referenceNumber)
        {
            return await _dbcontext.Evidences.AnyAsync(x => x.ReferenceNumber == referenceNumber);
        }

        public Task<EvidenceServiceResult> UpdateAsync(Core.Models.Evidence evidence)
        {
            throw new NotImplementedException();
        }
    }
}
