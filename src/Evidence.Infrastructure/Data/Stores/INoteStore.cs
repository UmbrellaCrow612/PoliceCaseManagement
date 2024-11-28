using Evidence.Infrastructure.Data.Models;

namespace Evidence.Infrastructure.Data.Stores
{
    public interface INoteStore
    {
        Task<IQueryable<Note>> Notes { get; }

        Task<(bool Succeeded, IEnumerable<string> Errors)> CreateNoteAsync(EvidenceItem evidence, Note note);
        Task<ICollection<Note>> GetNotesAsync(EvidenceItem evidence);
        Task<Note?> GetNoteByIdAsync(EvidenceItem evidence, string noteId);
        Task<(bool Succeeded, IEnumerable<string> Errors)> UpdateNoteAsync(EvidenceItem evidence, Note note);
        Task<(bool Succeeded, IEnumerable<string> Errors)> DeleteNoteAsync(EvidenceItem evidence, Note note);
    }
}
