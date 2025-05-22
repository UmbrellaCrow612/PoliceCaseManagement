using Identity.Core.Services;
using Identity.Infrastructure.Data;

namespace Identity.Application.Implementations
{
    public class UserService(IdentityApplicationDbContext dbContext) : IUserService
    {
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;
    }
}
