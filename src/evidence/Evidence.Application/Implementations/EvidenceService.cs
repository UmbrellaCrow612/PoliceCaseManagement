using Evidence.Core.Services;
using Evidence.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Evidence.Application.Implementations
{
    public class EvidenceService(EvidenceApplicationDbContext dbContext) : IEvidenceService
    {
        private readonly EvidenceApplicationDbContext _dbcontext = dbContext;


        public async Task<bool> ExistsAsync(string evidenceId)
        {
            return await _dbcontext.Evidences.AnyAsync(x => x.Id == evidenceId);
        }

        public async Task<Core.Models.Evidence?> FindByIdAsync(string evidenceId)
        {
            return await _dbcontext.Evidences.FindAsync(evidenceId);
        }
    }
}
