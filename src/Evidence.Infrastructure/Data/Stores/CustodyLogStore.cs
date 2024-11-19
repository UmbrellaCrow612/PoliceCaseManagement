using Evidence.Infrastructure.Data.Models;

namespace Evidence.Infrastructure.Data.Stores
{
    public class CustodyLogStore : ICustodyLogStore
    {
        public Task<(bool Succeeded, IEnumerable<string> Errors)> CreateCustodyLog(EvidenceItem evidence, CustodyLog custodyLog)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCustodyLog(EvidenceItem evidence, CustodyLog custodyLog)
        {
            throw new NotImplementedException();
        }

        public Task<CustodyLog?> GetCustodyLogById(EvidenceItem evidence, string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CustodyLog>> GetCustodyLogs(EvidenceItem evidence)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCustodyLog(EvidenceItem evidence, CustodyLog custodyLog)
        {
            throw new NotImplementedException();
        }
    }
}
