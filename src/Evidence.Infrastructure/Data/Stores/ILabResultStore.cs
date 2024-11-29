using Evidence.Infrastructure.Data.Models;

namespace Evidence.Infrastructure.Data.Stores
{
    public interface ILabResultStore
    {
        IQueryable<LabResult> LabResults { get; }

        Task<(bool Succeeded, ICollection<string> Errors)> CreateLabResultAsync(EvidenceItem evidence, LabResult result);
        Task<LabResult?> GetLabResultByIdAsync(EvidenceItem evidence, string labResultId);
        Task<(bool Succeeded, ICollection<string> Errors)> UpdateLabResultAsync(EvidenceItem evidence, LabResult result);
        Task<(bool Succeeded, ICollection<string> Errors)> DeleteLabResultAsync(EvidenceItem evidence, LabResult result);
        Task<ICollection<LabResult>> GetLabResultsAsync(EvidenceItem evidence);
    }
}
