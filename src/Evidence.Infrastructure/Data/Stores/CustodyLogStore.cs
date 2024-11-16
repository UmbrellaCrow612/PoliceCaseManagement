using Evidence.Infrastructure.Data.Models;

namespace Evidence.Infrastructure.Data.Stores
{
    public class CustodyLogStore : ICustodyLogStore
    {
        public Task<(bool Succeeded, IEnumerable<string> Errors)> CreateCustodyLog(CustodyLog custodyLog, EvidenceItem evidence)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCustodyLog(CustodyLog custodyLog)
        {
            throw new NotImplementedException();
        }

        public Task<CustodyLog?> GetCustodyLogById(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCustodyLog(CustodyLog custodyLog)
        {
            throw new NotImplementedException();
        }
    }
}
