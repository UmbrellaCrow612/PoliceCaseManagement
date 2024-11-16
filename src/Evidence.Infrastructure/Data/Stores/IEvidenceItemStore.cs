using Evidence.Infrastructure.Data.Models;

namespace Evidence.Infrastructure.Data.Stores
{
    public interface IEvidenceItemStore
    {
        Task<(bool Succeeded, IEnumerable<string> Errors)> CreateEvidence(EvidenceItem evidence);
        Task UpdateEvidence(string userId, EvidenceItem evidence);
        Task<EvidenceItem?> GetEvidenceById (string id);
        Task DeleteEvidence(string userId, EvidenceItem evidence);
    }
}
