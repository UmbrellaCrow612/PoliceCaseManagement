using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Infrastructure.Interfaces;

namespace PoliceCaseManagement.Infrastructure.Data.Repositories
{
    public class UserRepository(ApplicationDbContext context) : BaseRepository<User>(context), IUserRepository
    {
        public Task<bool> EmailExists(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UsernameExists(string username)
        {
            throw new NotImplementedException();
        }
    }
}
