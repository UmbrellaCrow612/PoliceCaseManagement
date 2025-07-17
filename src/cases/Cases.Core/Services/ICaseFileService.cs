using Cases.Core.Models;
using Cases.Core.ValueObjects;
using Results.Abstractions;

namespace Cases.Core.Services
{
    /// <summary>
    /// Handles all <see cref="Case"/> <see cref="CaseAttachmentFile"/> business logic
    /// </summary>
    public interface ICaseFileService
    {
        Task<CaseAttachmentFile?> FindByIdAsync(string fileId);

        Task<IResult> UploadComplete(CaseAttachmentFile file);

        Task<(string preSignedUrl, string fileId)> AddAsync(Case @case, CaseAttachmentFileMetaData fileMetaData);

        Task<List<CaseAttachmentFile>> GetAsync(Case @case);

        Task<string> DownloadAsync(CaseAttachmentFile file);

        Task<IResult> DeleteAsync(CaseAttachmentFile file);
    }
}
