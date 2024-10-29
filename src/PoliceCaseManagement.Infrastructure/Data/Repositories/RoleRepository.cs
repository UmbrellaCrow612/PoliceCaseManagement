using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Infrastructure.Interfaces;

namespace PoliceCaseManagement.Infrastructure.Data.Repositories
{
    public class RoleRepository(ApplicationDbContext context) : BaseRepository<Role>(context), IRoleRepository
    {
    }
}
