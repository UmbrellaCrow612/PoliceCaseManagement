using Evidence.Infrastructure.Data.Models;

namespace Evidence.Infrastructure.Data.Stores
{
    public class NoteStore : INoteStore
    {
        public Task<IQueryable<Note>> Notes => throw new NotImplementedException();

        public Task<(bool Succeeded, IEnumerable<string> Errors)> CreateNoteAsync(EvidenceItem evidence, Note note)
        {
            throw new NotImplementedException();
        }

        public Task DeleteNoteAsync(EvidenceItem evidence, Note note)
        {
            throw new NotImplementedException();
        }

        public Task<Note> GetNoteByIdAsync(EvidenceItem evidence, string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Note>> GetNotesAsync(EvidenceItem evidence)
        {
            throw new NotImplementedException();
        }

        public Task UpdateNoteAsync(EvidenceItem evidence, Note note)
        {
            throw new NotImplementedException();
        }

        Task<(bool Succeeded, IEnumerable<string> Errors)> INoteStore.DeleteNoteAsync(EvidenceItem evidence, Note note)
        {
            throw new NotImplementedException();
        }

        Task<ICollection<Note>> INoteStore.GetNotesAsync(EvidenceItem evidence)
        {
            throw new NotImplementedException();
        }

        Task<(bool Succeeded, IEnumerable<string> Errors)> INoteStore.UpdateNoteAsync(EvidenceItem evidence, Note note)
        {
            throw new NotImplementedException();
        }
    }
}
