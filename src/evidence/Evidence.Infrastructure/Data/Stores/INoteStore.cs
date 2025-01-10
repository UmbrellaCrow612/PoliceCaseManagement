using Evidence.Infrastructure.Data.Models;

namespace Evidence.Infrastructure.Data.Stores
{
    public interface INoteStore
    {
        IQueryable<Note> Notes { get; }

        Task<(bool Succeeded, ICollection<string> Errors)> CreateNoteAsync(EvidenceItem evidence, Note note);
        Task<ICollection<Note>> GetNotesAsync(EvidenceItem evidence);
        Task<Note?> GetNoteByIdAsync(EvidenceItem evidence, string noteId);
        Task<(bool Succeeded, ICollection<string> Errors)> UpdateNoteAsync(EvidenceItem evidence, Note note);
        Task<(bool Succeeded, ICollection<string> Errors)> DeleteNoteAsync(EvidenceItem evidence, Note note);
    }
}
