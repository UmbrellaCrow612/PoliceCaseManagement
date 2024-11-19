using Evidence.Infrastructure.Data.Models;

namespace Evidence.Infrastructure.Data.Stores
{
    public interface INoteStore
    {
        Task<(bool Succeeded, IEnumerable<string> Errors)> CreateNote(EvidenceItem evidence, Note note);
        Task<IEnumerable<Note>> GetNotes(EvidenceItem evidence);
        Task<Note> GetNoteById(EvidenceItem evidence, string id);
        Task UpdateNote(EvidenceItem evidence, Note note);
        Task DeleteNote(EvidenceItem evidence, Note note);
    }
}
