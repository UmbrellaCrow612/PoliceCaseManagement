using Microsoft.EntityFrameworkCore;
using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Infrastructure.Interfaces;

namespace PoliceCaseManagement.Infrastructure.Data.Repositories
{
    public class CaseRepository(ApplicationDbContext context) : BaseRepository<Case>(context), ICaseRepository
    {
        public async Task DeleteAsync(string id, string userId)
        {
            var caseToSoftDelete = await _context.Cases.FirstOrDefaultAsync(c => c.Id == id) ?? throw new ApplicationException("Case not found");

            caseToSoftDelete.IsDeleted = true;
            caseToSoftDelete.DeletedAt = DateTime.UtcNow;
            caseToSoftDelete.DeletedById = userId;

            await _context.SaveChangesAsync();
        }
    }
}
