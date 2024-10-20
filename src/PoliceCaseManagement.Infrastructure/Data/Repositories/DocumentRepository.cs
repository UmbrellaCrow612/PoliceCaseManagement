using Microsoft.EntityFrameworkCore;
using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Core.Interfaces;

namespace PoliceCaseManagement.Infrastructure.Data.Repositories
{
    public class DocumentRepository(ApplicationDbContext context) : IDocumentRepository<Document, string>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task AddAsync(Document entity)
        {
            await _context.Documents.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(string id, string userId)
        {
            var documentToDelete = await _context.Documents.FirstOrDefaultAsync(x => x.Id == id);
            if (documentToDelete is null) return false;

            documentToDelete.IsDeleted = true;
            documentToDelete.DeletedAt = DateTime.UtcNow;
            documentToDelete.DeletedById = userId;

            await _context.SaveChangesAsync();
            return true;
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(string id)
        {
            var documentExists = await _context.Documents.AnyAsync(x => x.Id == id);

            return documentExists;
        }

        public async Task<Document?> GetByIdAsync(string id)
        {
            var documentToGet = await _context.Documents.FirstOrDefaultAsync(x => x.Id == id);

            return documentToGet;
        }

        public async Task UpdateAsync(Document updatedEntity)
        {
            _context.Documents.Update(updatedEntity);
            await _context.SaveChangesAsync();
        }
    }
}
