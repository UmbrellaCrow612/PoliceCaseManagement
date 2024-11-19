using Evidence.Infrastructure.Data.Models;

namespace Evidence.Infrastructure.Data.Stores
{
    public interface ICustodyLogStore
    {
        Task<(bool Succeeded, IEnumerable<string> Errors)> CreateCustodyLog(CustodyLog custodyLog, EvidenceItem evidence);
        Task DeleteCustodyLog(EvidenceItem evidence, CustodyLog custodyLog);
        Task<CustodyLog?> GetCustodyLogById (EvidenceItem evidence, string id);
        Task<IEnumerable<CustodyLog>> GetCustodyLogs(EvidenceItem evidence);
        Task UpdateCustodyLog(EvidenceItem evidence, CustodyLog custodyLog);
    }
}
