using Evidence.Infrastructure.Data.Models;

namespace Evidence.Infrastructure.Data.Stores
{
    public interface ICustodyLogStore
    {
        IQueryable<CustodyLog> CustodyLogs { get; }

        Task<(bool Succeeded, ICollection<string> Errors)> CreateCustodyLog(EvidenceItem evidence, CustodyLog custodyLog);
        Task DeleteCustodyLog(EvidenceItem evidence, CustodyLog custodyLog);
        Task<CustodyLog?> GetCustodyLogById (EvidenceItem evidence, string custodyLogId);
        Task<ICollection<CustodyLog>> GetCustodyLogs(EvidenceItem evidence);
        Task UpdateCustodyLog(EvidenceItem evidence, CustodyLog custodyLog);
    }
}
