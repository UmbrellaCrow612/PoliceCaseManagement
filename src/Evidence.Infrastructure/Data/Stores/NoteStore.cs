using Evidence.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Evidence.Infrastructure.Data.Stores
{
    public class NoteStore(EvidenceApplicationDbContext dbContext) : INoteStore
    {
        private readonly EvidenceApplicationDbContext _dbContext = dbContext;

        public IQueryable<Note> Notes => _dbContext.Notes.AsQueryable();

        public async Task<(bool Succeeded, ICollection<string> Errors)> CreateNoteAsync(EvidenceItem evidence, Note note)
        {
            List<string> errors = [];

            var evidenceInContext = _dbContext.Evidences.Local.FirstOrDefault(x => x.Id == evidence.Id);
            if(evidenceInContext is null)
            {
                errors.Add("Evidence not in context");
                return (false, errors);
            }

            if(note.EvidenceItemId != evidence.Id)
            {
                errors.Add("Note is not linked to the given evidence");
                return (false, errors);
            }

            await _dbContext.Notes.AddAsync(note);
            await _dbContext.SaveChangesAsync();

            return (true, errors);
        }

        public async Task<(bool Succeeded, ICollection<string> Errors)> DeleteNoteAsync(EvidenceItem evidence, Note note)
        {
            List<string> errors = [];

            var evidenceInContext = _dbContext.Evidences.Local.FirstOrDefault(x => x.Id == evidence.Id);
            if (evidenceInContext is null)
            {
                errors.Add("Evidence not in context");
                return (false, errors);
            }

            var noteInContext = _dbContext.Notes.Local.FirstOrDefault(x => x.Id == note.Id);
            if (noteInContext is null)
            {
                errors.Add("Note not in context");
                return (false, errors);
            }

            if(note.EvidenceItemId != evidence.Id)
            {
                errors.Add("Note is not linked to the given evidence");
                return (false, errors);
            }

            _dbContext.Notes.Remove(note);
            await _dbContext.SaveChangesAsync();

            return (true, errors);
        }

        public async Task<Note?> GetNoteByIdAsync(EvidenceItem evidence, string noteId)
        {
            return await _dbContext.Notes.Where(x => x.Id == noteId && x.EvidenceItemId == evidence.Id).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Note>> GetNotesAsync(EvidenceItem evidence)
        {
            return (ICollection<Note>)await _dbContext.Evidences.Where(x => x.Id == evidence.Id).Include(x => x.Notes).Select(x => x.Notes).ToListAsync();
        }

        public async Task<(bool Succeeded, ICollection<string> Errors)> UpdateNoteAsync(EvidenceItem evidence, Note note)
        {
            List<string> errors = [];

            var evidenceInContext = _dbContext.Evidences.Local.FirstOrDefault(x => x.Id == evidence.Id);
            if (evidenceInContext is null)
            {
                errors.Add("Evidence not in context");
                return (false, errors);
            }

            var noteInContext = _dbContext.Notes.Local.FirstOrDefault(x => x.Id == note.Id);
            if (noteInContext is null)
            {
                errors.Add("Note not in context");
                return (false, errors);
            }

            if (note.EvidenceItemId != evidence.Id)
            {
                errors.Add("Note is not linked to the given evidence");
                return (false, errors);
            }

            _dbContext.Notes.Update(note);
            await _dbContext.SaveChangesAsync();

            return (true, errors);
        }
    }
}
