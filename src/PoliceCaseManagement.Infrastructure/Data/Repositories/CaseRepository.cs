using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Infrastructure.Interfaces;

namespace PoliceCaseManagement.Infrastructure.Data.Repositories
{
    public class CaseRepository(ApplicationDbContext context) : ICaseRepository
    {
        private readonly ApplicationDbContext _context = context;

        public Task<Case> AddAsync(Case entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Case entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Case?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Case entity)
        {
            throw new NotImplementedException();
        }
    }
}
