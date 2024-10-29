using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Infrastructure.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> UsernameExists(string username); 
        Task<bool> EmailExists (string email);
    }
}
