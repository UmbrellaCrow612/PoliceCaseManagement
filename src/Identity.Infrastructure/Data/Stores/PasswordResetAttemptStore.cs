using Identity.Infrastructure.Data.Models;
using Microsoft.Extensions.Configuration;

namespace Identity.Infrastructure.Data.Stores
{
    public class PasswordResetAttemptStore(IConfiguration configuration, IdentityApplicationDbContext dbContext) : IPasswordResetAttemptStore
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;

        public Task<(bool canMakeAttempt, bool successfullyAdded)> AddAttempt(PasswordResetAttempt attempt)
        {
            


            throw new NotImplementedException();
        }

        public Task<bool> RevokePasswordAttempt(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
