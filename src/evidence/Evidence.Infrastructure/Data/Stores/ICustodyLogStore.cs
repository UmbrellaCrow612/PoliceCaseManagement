using Evidence.Infrastructure.Data.Models;

namespace Evidence.Infrastructure.Data.Stores
{
    public interface ICustodyLogStore
    {
        IQueryable<CustodyLog> CustodyLogs { get; }

        Task<(bool Succeeded, ICollection<string> Errors)> CreateCustodyLogAsync(EvidenceItem evidence, CustodyLog custodyLog);
        Task<CustodyLog?> GetCustodyLogByIdAsync(EvidenceItem evidence, string custodyLogId);
        Task<(bool Succeeded, ICollection<string> Errors)> UpdateCustodyLogAsync(EvidenceItem evidence, CustodyLog custodyLog);
        Task<(bool Succeeded, ICollection<string> Errors)> DeleteCustodyLogAsync(EvidenceItem evidence, CustodyLog custodyLog);
        Task<ICollection<CustodyLog>> GetCustodyLogsAsync(EvidenceItem evidence);
       
    }
}
