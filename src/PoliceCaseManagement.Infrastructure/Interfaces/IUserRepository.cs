using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Infrastructure.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> UsernameExistsAsync(string username); 
        Task<bool> EmailExistsAsync (string email);
    }
}
