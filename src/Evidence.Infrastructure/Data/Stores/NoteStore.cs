using Evidence.Infrastructure.Data.Models;

namespace Evidence.Infrastructure.Data.Stores
{
    public class NoteStore : INoteStore
    {
        public Task<(bool Succeeded, IEnumerable<string> Errors)> CreateNote(EvidenceItem evidence, Note note)
        {
            throw new NotImplementedException();
        }

        public Task DeleteNote(EvidenceItem evidence, Note note)
        {
            throw new NotImplementedException();
        }

        public Task<Note> GetNoteById(EvidenceItem evidence, string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Note>> GetNotes(EvidenceItem evidence)
        {
            throw new NotImplementedException();
        }

        public Task UpdateNote(EvidenceItem evidence, Note note)
        {
            throw new NotImplementedException();
        }
    }
}
