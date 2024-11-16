using Evidence.Infrastructure.Data.Models;

namespace Evidence.Infrastructure.Data.Stores
{
    public interface ICustodyLogStore
    {
        Task<(bool Succeeded, IEnumerable<string> Errors)> CreateCustodyLog(CustodyLog custodyLog, EvidenceItem evidence);
        Task DeleteCustodyLog(CustodyLog custodyLog);
        Task<CustodyLog?> GetCustodyLogById (string id);
        Task UpdateCustodyLog(CustodyLog custodyLog);
    }
}
