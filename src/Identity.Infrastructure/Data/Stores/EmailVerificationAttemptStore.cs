using Identity.Infrastructure.Data.Models;

namespace Identity.Infrastructure.Data.Stores
{
    internal class EmailVerificationAttemptStore(IdentityApplicationDbContext dbContext) : IEmailVerificationAttemptStore
    {
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;

        public async Task<(bool canMakeAttempt, bool successfullyAdded)> AddAttempt(EmailVerificationAttempt attempt)
        {
            // check if there is valid attempt already made - retrun false
            // we can make an attempt add it to db and return true
            return (true, true);
        }

        public void SetToUpdateAttempt(EmailVerificationAttempt attempt)
        {
            _dbcontext.EmailVerificationAttempts.Update(attempt);
        }
    }
}
