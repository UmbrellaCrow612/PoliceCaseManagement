using Evidence.Infrastructure.Data.Models;

namespace Evidence.Infrastructure.Data.Stores
{
    public interface IEvidenceItemStore
    {
        IQueryable<EvidenceItem> Evidences { get; }

        Task<(bool Succeeded, ICollection<string> Errors)> CreateEvidenceAsync(EvidenceItem evidence);
        Task<EvidenceItem?> GetEvidenceByIdAsync(string evidenceId);
        Task<(bool Succeeded, ICollection<string> Errors)> UpdateEvidenceAsync(string userId, EvidenceItem evidence);
        Task<(bool Succeeded, ICollection<string> Errors)> DeleteEvidenceAsync(string userId, EvidenceItem evidence);
    }
}
