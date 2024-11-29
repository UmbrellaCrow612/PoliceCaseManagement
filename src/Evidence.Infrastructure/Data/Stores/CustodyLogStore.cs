using Evidence.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Evidence.Infrastructure.Data.Stores
{
    public class CustodyLogStore(EvidenceApplicationDbContext dbContext) : ICustodyLogStore
    {
        private readonly EvidenceApplicationDbContext _dbcontext = dbContext;
        public IQueryable<CustodyLog> CustodyLogs => _dbcontext.CustodyLogs.AsQueryable();

        public async Task<(bool Succeeded, ICollection<string> Errors)> CreateCustodyLogAsync(EvidenceItem evidence, CustodyLog custodyLog)
        {
            List<string> errors = [];

            var evidenceInContext = _dbcontext.Evidences.Local.FirstOrDefault(x => x.Id == evidence.Id);
            if(evidenceInContext is null) errors.Add("Evidence not in context.");

            if(custodyLog.EvidenceItemId != evidence.Id) errors.Add("CustodyLog is not linked to evidence.");

            if (errors.Count != 0) return (false, errors);

            await _dbcontext.CustodyLogs.AddAsync(custodyLog);
            await _dbcontext.SaveChangesAsync();

            return (true, errors);
        }

        public async Task<(bool Succeeded, ICollection<string> Errors)> DeleteCustodyLogAsync(EvidenceItem evidence, CustodyLog custodyLog)
        {
            List<string> errors = [];

            var evidenceInContext = _dbcontext.Evidences.Local.FirstOrDefault(x => x.Id == evidence.Id);
            if (evidenceInContext is null) errors.Add("Evidence not in context.");
            
            var custodyLogInContext = _dbcontext.CustodyLogs.Local.FirstOrDefault(x => x.Id == custodyLog.Id);
            if (custodyLogInContext is null) errors.Add("CustodyLog not in context.");

            if (custodyLog.EvidenceItemId != evidence.Id) errors.Add("CustodyLog is not linked to evidence.");

            if(errors.Count != 0) return (false, errors);

            _dbcontext.CustodyLogs.Remove(custodyLog);
            await _dbcontext.SaveChangesAsync();

            return (true, errors);
        }

        public async Task<CustodyLog?> GetCustodyLogByIdAsync(EvidenceItem evidence, string custodyLogId)
        {
            return await _dbcontext.CustodyLogs.Where(x => x.Id == custodyLogId && x.EvidenceItemId == evidence.Id).FirstOrDefaultAsync();
        }

        public async Task<ICollection<CustodyLog>> GetCustodyLogsAsync(EvidenceItem evidence)
        {
            return await _dbcontext.CustodyLogs.Where(x => x.EvidenceItemId == evidence.Id).ToListAsync();
        }

        public async Task<(bool Succeeded, ICollection<string> Errors)> UpdateCustodyLogAsync(EvidenceItem evidence, CustodyLog custodyLog)
        {
            List<string> errors = [];

            var evidenceInContext = _dbcontext.Evidences.Local.FirstOrDefault(x => x.Id == evidence.Id);
            if (evidenceInContext is null) errors.Add("Evidence not in context.");

            var custodyLogInContext = _dbcontext.CustodyLogs.Local.FirstOrDefault(x => x.Id == custodyLog.Id);
            if (custodyLogInContext is null) errors.Add("CustodyLog not in context.");

            if (custodyLog.EvidenceItemId != evidence.Id) errors.Add("CustodyLog is not linked to evidence.");

            if (errors.Count != 0) return (false, errors);

            _dbcontext.CustodyLogs.Update(custodyLog);
            await _dbcontext.SaveChangesAsync();

            return (true, errors);
        }
    }
}
