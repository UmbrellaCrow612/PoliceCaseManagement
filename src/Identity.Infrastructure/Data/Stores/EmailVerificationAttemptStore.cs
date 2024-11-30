using Identity.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data.Stores
{
    internal class EmailVerificationAttemptStore(IdentityApplicationDbContext dbContext, UserManager<ApplicationUser> userManager) : IEmailVerificationAttemptStore
    {
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<(bool canMakeAttempt, bool successfullyAdded)> AddAttemptAsync(EmailVerificationAttempt attempt)
        {
            var recentValidAttempt = await _dbcontext.EmailVerificationAttempts
                .Where(x => 
                x.IsUsed == false && 
                x.ExpiresAt > DateTime.UtcNow.AddMinutes(-30) &&
                x.UsedAt == null &&
                x.UserId == attempt.UserId
                )
                .OrderByDescending(x => x.ExpiresAt)
                .FirstOrDefaultAsync();

            if (recentValidAttempt is not null) return (false, false);

            await _dbcontext.EmailVerificationAttempts.AddAsync(attempt);
            await _dbcontext.SaveChangesAsync();

            return (true, true);
        }

        public void SetToUpdateAttempt(EmailVerificationAttempt attempt)
        {
            _dbcontext.EmailVerificationAttempts.Update(attempt);
        }

        public async Task<(bool isValid, EmailVerificationAttempt? attempt, ApplicationUser? user, ICollection<string> errors)> ValidateAttemptAsync(string email, string code)
        {
            List<string> errors = [];

            var user = await _userManager.FindByEmailAsync(email);
            if(user is null)
            {
                errors.Add("User dose not exist.");
                return (false, null, null, errors);
            }

            var _attempt = await _dbcontext.EmailVerificationAttempts
                .FirstOrDefaultAsync(x => x.Email == email && x.Code == code && x.IsUsed == false);
            if (_attempt is null)
            {
                errors.Add("Attempt not found.");
                return (false, null,null, errors);
            }

            if(!_attempt.IsValid())
            {
                errors.Add("Attempt invalid.");
                return (false, null, null, errors);
            }

            return (true, _attempt, user, errors);
        }
    }
}
