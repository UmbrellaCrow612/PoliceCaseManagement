using Evidence.Infrastructure.Data.Models;

namespace Evidence.Infrastructure.Data.Stores
{
    public interface ILabResultStore
    {
        Task<IQueryable<LabResult>> LabResults { get; }

        Task<(bool Succeeded, IEnumerable<string> Errors)> CreateLabResult(EvidenceItem evidence, LabResult result);
        Task<IEnumerable<LabResult>> GetLabResults(EvidenceItem evidence);
        Task<LabResult> GetLabResultById(EvidenceItem evidence, string id);
        Task UpdateLabResult(EvidenceItem evidence, LabResult result);
        Task DeleteLabResult(EvidenceItem evidence, LabResult result);
    }
}
