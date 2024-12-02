using Identity.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data.Stores
{
    public class UserDeviceChallengeAttemptStore(IdentityApplicationDbContext dbContext) : IUserDeviceChallengeAttemptStore
    {
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;

        public IQueryable<UserDeviceChallengeAttempt> UserDeviceChallengeAttempts => _dbcontext.UserDeviceChallengeAttempts.AsQueryable();

        public async Task<(bool canMakeAttempt, ICollection<string> Errors)> AddAttempt(UserDeviceChallengeAttempt attempt)
        {
            List<string> errors = [];

            var validRecentAttempt = await _dbcontext.UserDeviceChallengeAttempts
                .Where(x => x.UserId == attempt.UserId && x.IsSuccessful == false 
                && x.SuccessfulAt == null 
                && x.CreatedAt.AddMinutes(30) > DateTime.UtcNow)
                .OrderBy(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            if(validRecentAttempt is not null)
            {
                errors.Add("Valid recent attempt already issued out.");
                return (false, errors);
            } 

            await _dbcontext.UserDeviceChallengeAttempts.AddAsync(attempt);
            await _dbcontext.SaveChangesAsync();

            return (true, errors);
        }

        public Task<(bool isValid, UserDeviceChallengeAttempt? attempt)> ValidateAttempt(string email, string code)
        {
            throw new NotImplementedException();
        }
    }
}
