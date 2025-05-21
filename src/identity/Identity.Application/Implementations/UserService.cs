using Identity.Application.Codes;
using Identity.Core.Services;
using Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.Implementations
{
    public class UserService(IdentityApplicationDbContext dbContext) : IUserService
    {
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;

        public async Task<BulkFetchUsersResult> BulkFetchUsers(List<string> userIds)
        {
            var result = new BulkFetchUsersResult();

            foreach (var id in userIds)
            {
                var exists = await _dbcontext.Users.AnyAsync(x => x.Id == id);
                if (!exists)
                {
                    result.AddError(BusinessRuleCodes.UserDoesNotExist, "User dose not exist");
                    return result;
                }
            }
            result.Succeeded = true;
            result.Users = await _dbcontext.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();

            return result;
        }
    }
}
