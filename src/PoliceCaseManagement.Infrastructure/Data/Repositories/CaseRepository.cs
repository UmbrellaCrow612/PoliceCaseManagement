using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Core.Interfaces;

namespace PoliceCaseManagement.Infrastructure.Data.Repositories
{
    public class CaseRepository : ICaseRepository
    {
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

        public Task<ICollection<Case>> Search()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Case entity)
        {
            throw new NotImplementedException();
        }
    }
}
