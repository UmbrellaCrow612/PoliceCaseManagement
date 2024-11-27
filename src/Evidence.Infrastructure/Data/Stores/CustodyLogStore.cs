using Evidence.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Evidence.Infrastructure.Data.Stores
{
    public class CustodyLogStore(EvidenceApplicationDbContext dbContext) : ICustodyLogStore
    {
        private readonly EvidenceApplicationDbContext _dbcontext = dbContext;
        public IQueryable<CustodyLog> CustodyLogs => _dbcontext.CustodyLogs.AsQueryable();

        public async Task<(bool Succeeded, ICollection<string> Errors)> CreateCustodyLog(EvidenceItem evidence, CustodyLog custodyLog)
        {
            _ = _dbcontext.Evidences.Local.FirstOrDefault(x => x.Id == evidence.Id) ?? throw new ApplicationException("Evidence not in context");
            _ = _dbcontext.CustodyLogs.Local.FirstOrDefault(x => x.Id == custodyLog.Id) ?? throw new ApplicationException("Custody log not in context");

            if (custodyLog.EvidenceItemId != evidence.Id) throw new ApplicationException("Custody log not linked to evidence.");

            await _dbcontext.CustodyLogs.AddAsync(custodyLog);

            await _dbcontext.SaveChangesAsync();

            return (true, []);
        }

        public async Task DeleteCustodyLog(EvidenceItem evidence, CustodyLog custodyLog)
        {
            _ = _dbcontext.Evidences.Local.FirstOrDefault(x => x.Id == evidence.Id) ?? throw new ApplicationException("Evidence not in context");
            _ = _dbcontext.CustodyLogs.Local.FirstOrDefault(x => x.Id == custodyLog.Id) ?? throw new ApplicationException("Custody log not in context");

            if (custodyLog.EvidenceItemId != evidence.Id) throw new ApplicationException("Custody log and evidence not linked.");

            _dbcontext.CustodyLogs.Remove(custodyLog);

            await _dbcontext.SaveChangesAsync();
        }

        public async Task<CustodyLog?> GetCustodyLogById(EvidenceItem evidence, string custodyLogId)
        {
            return await _dbcontext.CustodyLogs.FirstOrDefaultAsync(x => x.Id == custodyLogId && x.EvidenceItemId == evidence.Id);
        }

        public async Task<ICollection<CustodyLog>> GetCustodyLogs(EvidenceItem evidence)
        {
            _ = _dbcontext.Evidences.Local.FirstOrDefault(x => x.Id == evidence.Id) ?? throw new ApplicationException("Evidence is not in context.");

            var logs = await _dbcontext.CustodyLogs.Where(x => x.EvidenceItemId == evidence.Id).ToListAsync();

            return logs;
        }

        public async Task UpdateCustodyLog(EvidenceItem evidence, CustodyLog custodyLog)
        {
            _ = _dbcontext.Evidences.Local.FirstOrDefault(x => x.Id == evidence.Id) ?? throw new ApplicationException("evidence is not context.");
            _ = _dbcontext.CustodyLogs.Local.FirstOrDefault(x => x.Id == custodyLog.Id) ?? throw new ApplicationException("custody log is not context.");

            if (custodyLog.EvidenceItemId != evidence.Id) throw new ApplicationException("Custody log is not linked to evidence.");

            _dbcontext.CustodyLogs.Update(custodyLog);

            await _dbcontext.SaveChangesAsync();
        }
    }
}
