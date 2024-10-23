using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Infrastructure.Interfaces
{
    public interface ICaseRepository : IRepository<Case>
    {
        Task DeleteAsync(string id, string userId);
    }
}
