using Identity.Core.Models;
using Identity.Infrastructure.Data.Stores.Interfaces;
using Identity.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Utils.DTOs;

namespace Identity.Infrastructure.Data.Stores
{
    public class PasswordResetAttemptStore(IdentityApplicationDbContext dbContext, IOptions<TimeWindows> timeWindows) : IPasswordResetAttemptStore
    {
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;
        private readonly TimeWindows _timeWindows = timeWindows.Value;
        public IQueryable<PasswordResetAttempt> PasswordResetAttempts => _dbcontext.PasswordResetAttempts.AsQueryable();

        public async Task<(bool canMakeAttempt, ICollection<ErrorDetail> errors)> AddAttempt(PasswordResetAttempt attempt)
        {
            List<ErrorDetail> errors = [];

            var validTime = _timeWindows.ResetPasswordTime;

            var validRecentAttempt = await _dbcontext.PasswordResetAttempts
                .Where(x => x.UserId == attempt.UserId
                 && x.IsUsed == false
                 && x.IsRevoked == false
                 && x.CreatedAt.AddMinutes(validTime) > DateTime.UtcNow)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            if (validRecentAttempt is not null)
            {
                errors.Add(new ErrorDetail
                {
                    Field = "Password reset attempt",
                    Reason = "There is at least one valid recent password attempt"
                });

                return (false, errors);
            }

            await _dbcontext.PasswordResetAttempts.AddAsync(attempt);
            await _dbcontext.SaveChangesAsync();

            return (true, errors);
        }

        public async Task<int> RevokeAllValidPasswordAttempts(string userId)
        {
            var validTime = _timeWindows.ResetPasswordTime;

            var validRecentAttempts = await _dbcontext.PasswordResetAttempts
             .Where(
              x => x.UserId == userId
              && x.IsUsed == false
              && x.IsRevoked == false
              && x.CreatedAt.AddMinutes(validTime) > DateTime.UtcNow
              )
             .ToListAsync();

            foreach (var attempt in validRecentAttempts)
            {
                attempt.Revoke();
            }

            var count = validRecentAttempts.Count;

            _dbcontext.PasswordResetAttempts.UpdateRange(validRecentAttempts);
            await _dbcontext.SaveChangesAsync();

            return count;
        }

        public void SetToUpdateAttempt(PasswordResetAttempt attempt)
        {
            _dbcontext.PasswordResetAttempts.Update(attempt);
        }

        public async Task<(bool isValid, PasswordResetAttempt? attempt)> ValidateAttempt(string code)
        {
            var validTime = _timeWindows.ResetPasswordTime;

            var _attempt = await _dbcontext.PasswordResetAttempts.FirstOrDefaultAsync(x => x.Code == code);
            if (_attempt is null) return (false, null);

            if (!_attempt.IsValid()) return (false, null);

            return (true, _attempt);
        }
    }
}
