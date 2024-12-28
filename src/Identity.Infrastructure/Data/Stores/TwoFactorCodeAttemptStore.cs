using Identity.Core.Models;
using Identity.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.DTOs;

namespace Identity.Infrastructure.Data.Stores
{
    public class TwoFactorCodeAttemptStore(IdentityApplicationDbContext dbContext, IOptions<TimeWindows> timeWindows) : ITwoFactorCodeAttemptStore
    {
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;
        private readonly TimeWindows _timeWindows = timeWindows.Value;

        public IQueryable<TwoFactorCodeAttempt> TwoFactorCodeAttempts => _dbcontext.TwoFactorCodeAttempts.AsQueryable();

        public async Task<(bool canMakeAttempt, ICollection<ErrorDetail> errors)> AddAttempt(TwoFactorCodeAttempt attempt)
        {
            List<ErrorDetail> errors = [];

            var validTime = _timeWindows.TwoFactorCodeTime;

            var validRecentAttemptForThisLogin = await _dbcontext.TwoFactorCodeAttempts
                .Where(x => x.LoginAttemptId == attempt.LoginAttemptId 
                && x.IsSuccessful == false 
                && x.CreatedAt.AddMinutes(validTime) > DateTime.UtcNow)
                .FirstOrDefaultAsync();

            if(validRecentAttemptForThisLogin is not null)
            {
                errors.Add(new ErrorDetail
                {
                    Field = "Two factor auth.",
                    Reason = "There is a valid recent attempt issued for this login attempt."
                });
                return (false, errors);
            }

            await _dbcontext.TwoFactorCodeAttempts.AddAsync(attempt);
            await _dbcontext.SaveChangesAsync();

            return (true, errors);
        }

        public void SetToUpdateAttempt(TwoFactorCodeAttempt attempt)
        {
            _dbcontext.TwoFactorCodeAttempts.Update(attempt);
        }

        public async Task<(bool isValid, TwoFactorCodeAttempt? attempt, ApplicationUser? user, ICollection<ErrorDetail> errors)> ValidateAttempt(string loginAttemptId, string code)
        {
            List<ErrorDetail> errors = [];
            var validTime = _timeWindows.TwoFactorCodeTime;

            var _attempt = await _dbcontext.TwoFactorCodeAttempts
                .Where(x => x.LoginAttemptId == loginAttemptId 
                && x.IsSuccessful == false && x.Code == code).FirstOrDefaultAsync();

            if(_attempt is null)
            {
                errors.Add(new ErrorDetail
                {
                    Field = "Two factor auth.",
                    Reason = "Attempt not found."
                });
                return (false, null, null, errors);
            }

            if (!_attempt.IsValid(validTime))
            {
                errors.Add(new ErrorDetail
                {
                    Field = "Two factor auth.",
                    Reason = "Attempt not valid."
                });
                return (false, null, null, errors);
            }

            var user = await _dbcontext.Users.FirstOrDefaultAsync(x => x.Id == _attempt.UserId);
            if(user is null)
            {
                errors.Add(new ErrorDetail
                {
                    Field = "Two factor auth.",
                    Reason = "User not found."
                });
                return (false, null, null, errors);
            }

            return (true, _attempt, user, errors);
        }
    }
}
