using Evidence.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Evidence.Infrastructure.Data.Stores
{
    public class LabResultStore(EvidenceApplicationDbContext dbContext) : ILabResultStore
    {
        private readonly EvidenceApplicationDbContext _dbContext = dbContext;

        public IQueryable<LabResult> LabResults => _dbContext.LabResults.AsQueryable();

        public async Task<(bool Succeeded, ICollection<string> Errors)> CreateLabResultAsync(EvidenceItem evidence, LabResult result)
        {
            List<string> errors = [];

            var evidenceInContext = _dbContext.Evidences.Local.FirstOrDefault(x => x.Id == evidence.Id);
            if(evidenceInContext is null) errors.Add("Evidence not in context.");

            if(result.EvidenceItemId != evidence.Id) errors.Add("LabResult not linked to evidence.");

            if(errors.Count != 0) return (false, errors);

            await _dbContext.LabResults.AddAsync(result);
            await _dbContext.SaveChangesAsync();

            return (true, errors);
        }

        public async Task<(bool Succeeded, ICollection<string> Errors)> DeleteLabResultAsync(EvidenceItem evidence, LabResult result)
        {
            List<string> errors = [];

            var evidenceInContext = _dbContext.Evidences.Local.FirstOrDefault(x => x.Id == evidence.Id);
            if (evidenceInContext is null) errors.Add("Evidence not in context.");

            var labResultInContext = _dbContext.LabResults.Local.FirstOrDefault(x => x.Id == result.Id);
            if (labResultInContext is null) errors.Add("LabResult not in context.");

            if(result.EvidenceItemId != evidence.Id) errors.Add("LabResult not linked to evidence");

            if (errors.Count != 0) return (false, errors);

            _dbContext.LabResults.Remove(result);
            await _dbContext.SaveChangesAsync();

            return (true, errors);
        }

        public async Task<LabResult?> GetLabResultByIdAsync(EvidenceItem evidence, string labResultId)
        {
            return await _dbContext.LabResults.Where(x => x.Id == labResultId && x.EvidenceItemId == evidence.Id).FirstOrDefaultAsync();
        }

        public async Task<ICollection<LabResult>> GetLabResultsAsync(EvidenceItem evidence)
        {
            return await _dbContext.LabResults.Where(x => x.EvidenceItemId == evidence.Id).ToListAsync();
        }

        public async Task<(bool Succeeded, ICollection<string> Errors)> UpdateLabResultAsync(EvidenceItem evidence, LabResult result)
        {
            List<string> errors = [];

            var evidenceInContext = _dbContext.Evidences.Local.FirstOrDefault(x => x.Id == evidence.Id);
            if (evidenceInContext is null) errors.Add("Evidence not in context.");

            var labResultInContext = _dbContext.LabResults.Local.FirstOrDefault(x => x.Id == result.Id);
            if (labResultInContext is null) errors.Add("LabResult not in context.");

            if (result.EvidenceItemId != evidence.Id) errors.Add("LabResult not linked to evidence");

            if (errors.Count != 0) return (false, errors);

            _dbContext.LabResults.Update(result);
            await _dbContext.SaveChangesAsync();

            return (true, errors);
        }
    }
}
