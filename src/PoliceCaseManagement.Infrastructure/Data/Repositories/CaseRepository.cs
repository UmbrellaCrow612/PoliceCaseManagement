using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Core.Interfaces;

namespace PoliceCaseManagement.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Represents a repository for managing cases.
    /// </summary>
    public class CaseRepository(ApplicationDbContext context) : ICaseRepository<Case, string>
    {
        private readonly ApplicationDbContext _context = context;
        public Task AddAsync(Case entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Case entity)
        {
            throw new NotImplementedException();
        }

        public Task<Case?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Case>> SearchAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Case entity)
        {
            throw new NotImplementedException();
        }
    }
}
