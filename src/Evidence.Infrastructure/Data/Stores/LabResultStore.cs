using Evidence.Infrastructure.Data.Models;

namespace Evidence.Infrastructure.Data.Stores
{
    public class LabResultStore : ILabResultStore
    {
        public Task<(bool Succeeded, IEnumerable<string> Errors)> CreateLabResult(EvidenceItem evidence, LabResult result)
        {
            throw new NotImplementedException();
        }

        public Task DeleteLabResult(EvidenceItem evidence, LabResult result)
        {
            throw new NotImplementedException();
        }

        public Task<LabResult> GetLabResultById(EvidenceItem evidence, string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LabResult>> GetLabResults(EvidenceItem evidence)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLabResult(EvidenceItem evidence, LabResult result)
        {
            throw new NotImplementedException();
        }
    }
}
