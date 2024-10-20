using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Core.Interfaces;

namespace PoliceCaseManagement.Infrastructure.Data.Repositories
{
    public class StatementRepository : IStatementRepository<Statement, string>
    {
        public Task AddAsync(Statement entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Statement?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Statement updatedEntity)
        {
            throw new NotImplementedException();
        }
    }
}
