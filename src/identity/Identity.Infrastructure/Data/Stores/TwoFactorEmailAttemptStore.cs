using Identity.Core.Models;
using Identity.Infrastructure.Data.Stores.Interfaces;
using Identity.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Utils.DTOs;

namespace Identity.Infrastructure.Data.Stores
{
    internal class TwoFactorEmailAttemptStore(IdentityApplicationDbContext dbContext, IOptions<TimeWindows> options) : ITwoFactorEmailAttemptStore
    {
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;
        private readonly TimeWindows _timeWindows = options.Value;

        public IQueryable<TwoFactorEmailAttempt> TwoFactorEmailAttempts => _dbcontext.TwoFactorEmailAttempts.AsQueryable();

        public async Task<(bool canMakeAttempt, List<ErrorDetail> errors)> AddAttempt(TwoFactorEmailAttempt attempt)
        {
            List<ErrorDetail> errors = [];

            var validRecentAttemptExists = await _dbcontext.TwoFactorEmailAttempts
                .AnyAsync(
                x => x.IsSuccessful == false
                && x.LoginAttemptId == attempt.LoginAttemptId
                && x.CreatedAt.AddMinutes(_timeWindows.TwoFactorEmailTime) > DateTime.UtcNow
            );

            if (validRecentAttemptExists)
            {
                errors.Add(new ErrorDetail
                {
                    Field = "Two factor email",
                    Reason = "There is a valid recent attempt."
                });

                return (false, errors);
            }

            await _dbcontext.TwoFactorEmailAttempts.AddAsync(attempt);
            await _dbcontext.SaveChangesAsync();

            return (true, errors);
        }


        public async Task<(bool isValid, List<ErrorDetail> errors)> ValidateAttempt(string loginAttemptId, string code)
        {
            List<ErrorDetail> errors = [];

            var loginAttempt = await _dbcontext.LoginAttempts.FindAsync(loginAttemptId);
            if (loginAttempt is null || !loginAttempt.IsValid())
            {
                errors.Add(new ErrorDetail
                {
                    Field = "Two factor email",
                    Reason = "Login attempt dose not exist or is invalid"
                });
            }


            var _attempt = await _dbcontext.TwoFactorEmailAttempts.Where(
                x => x.LoginAttemptId == loginAttemptId && x.Code == code)
                .FirstOrDefaultAsync();

            if (_attempt is null || !_attempt.IsValid(_timeWindows.TwoFactorEmailTime))
            {
                errors.Add(new ErrorDetail
                {
                    Field = "Two factor email",
                    Reason = "Two factor email attempt dose not exist for this login or code or is invalid"
                });
            }

            if (errors.Count != 0) return (false, errors);

            _attempt.MarkUsed();
            loginAttempt.Status = LoginStatus.SUCCESS;

            _dbcontext.LoginAttempts.Update(loginAttempt);
            _dbcontext.TwoFactorEmailAttempts.Update(_attempt);

            return (true, errors);
        }
    }
}
