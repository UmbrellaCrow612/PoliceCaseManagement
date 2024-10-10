using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Core.Interfaces
{
    public interface ICaseRepository : IGenericRepository<Case, string>
    {
        Task<ICollection<Case>> Search();
    }
}
