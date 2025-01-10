using Identity.Core.Models;
using Identity.Infrastructure.Data.Stores.Interfaces;
using Identity.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.DTOs;

namespace Identity.Infrastructure.Data.Stores
{
    public class TwoFactorSmsAttemptStore(IdentityApplicationDbContext dbContext, IOptions<TimeWindows> timeWindows) : ITwoFactorSmsAttemptStore
    {
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;
        private readonly TimeWindows _timeWindows = timeWindows.Value;

        public IQueryable<TwoFactorSmsAttempt> TwoFactorSmsAttempts => _dbcontext.TwoFactorSmsAttempts.AsQueryable();

        public async Task<(bool canMakeAttempt, ICollection<ErrorDetail> errors)> AddAttempt(TwoFactorSmsAttempt attempt)
        {
            List<ErrorDetail> errors = [];

            var validTime = _timeWindows.TwoFactorSmsTime;

            var validRecentAttemptForThisLogin = await _dbcontext.TwoFactorSmsAttempts
                .Where(x => x.LoginAttemptId == attempt.LoginAttemptId 
                && x.IsSuccessful == false 
                && x.CreatedAt.AddMinutes(validTime) > DateTime.UtcNow)
                .FirstOrDefaultAsync();

            if(validRecentAttemptForThisLogin is not null)
            {
                errors.Add(new ErrorDetail
                {
                    Field = "Two factor auth.",
                    Reason = "There is a valid recent SMS attempt issued for this login."
                });
                return (false, errors);
            }

            await _dbcontext.TwoFactorSmsAttempts.AddAsync(attempt);
            await _dbcontext.SaveChangesAsync();

            return (true, errors);
        }

        public void SetToUpdateAttempt(TwoFactorSmsAttempt attempt)
        {
            _dbcontext.TwoFactorSmsAttempts.Update(attempt);
        }

        public async Task<(bool isValid, ICollection<ErrorDetail> errors)> ValidateAttempt(string loginAttemptId, string code)
        {
            List<ErrorDetail> errors = [];
            var validTime = _timeWindows.TwoFactorSmsTime;

            var loginAttempt = await _dbcontext.LoginAttempts.FindAsync(loginAttemptId);
            if(loginAttempt is null || !loginAttempt.IsValid(_timeWindows.LoginLifetime))
            {
                errors.Add(new ErrorDetail
                {
                    Field = "Two facor sms authentication",
                    Reason = "Login attempt not found or is invalid."
                });
            }

            var _attempt = await _dbcontext.TwoFactorSmsAttempts
                .Where(x => x.LoginAttemptId == loginAttemptId && x.Code == code).FirstOrDefaultAsync();

            if(_attempt is null || !_attempt.IsValid(validTime))
            {
                errors.Add(new ErrorDetail
                {
                    Field = "Two factor auth.",
                    Reason = "Attempt not found or is invalid."
                });
            }

            if (errors.Count != 0) return (false, errors);

            loginAttempt.Status = LoginStatus.SUCCESS;
            _attempt.MarkUsed();

            return (true, errors);
        }
    }
}
