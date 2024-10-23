using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Infrastructure.Interfaces;

namespace PoliceCaseManagement.Infrastructure.Data.Repositories
{
    public class CaseRepository(ApplicationDbContext context) : BaseRepository<Case>(context), ICaseRepository
    {
        private readonly ApplicationDbContext _context = context;
        public async Task DeleteAsync(string id, string userId)
        {
           // Soft delete
        }
    }
}
