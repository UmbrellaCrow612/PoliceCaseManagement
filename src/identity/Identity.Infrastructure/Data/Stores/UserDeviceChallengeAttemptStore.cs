using Identity.Core.Models;
using Identity.Infrastructure.Data.Stores.Interfaces;
using Identity.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Utils;

namespace Identity.Infrastructure.Data.Stores
{
    public class UserDeviceChallengeAttemptStore(IdentityApplicationDbContext dbContext, IOptions<TimeWindows> timeWindows) : IUserDeviceChallengeAttemptStore
    {
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;
        private readonly TimeWindows _timeWindows = timeWindows.Value;

        public IQueryable<UserDeviceChallengeAttempt> UserDeviceChallengeAttempts => _dbcontext.UserDeviceChallengeAttempts.AsQueryable();

        public async Task<(bool canMakeAttempt, ICollection<string> Errors)> AddAttempt(UserDeviceChallengeAttempt attempt)
        {
            List<string> errors = [];
            var validTime = _timeWindows.DeviceChallengeTime;

            var validRecentAttempt = await _dbcontext.UserDeviceChallengeAttempts
                .Where(x => x.UserId == attempt.UserId && x.IsSuccessful == false 
                && x.SuccessfulAt == null 
                && x.UserDeviceId == attempt.UserDeviceId
                && x.CreatedAt.AddMinutes(validTime) > DateTime.UtcNow)
                .OrderBy(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            if(validRecentAttempt is not null)
            {
                errors.Add("Valid recent attempt already issued out for this device.");
                return (false, errors);
            } 

            await _dbcontext.UserDeviceChallengeAttempts.AddAsync(attempt);
            await _dbcontext.SaveChangesAsync();

            return (true, errors);
        }

        public void SetToUpdateAttempt(UserDeviceChallengeAttempt attempt)
        {
            if (!EfHelper.ExistsInContext(_dbcontext, attempt)) throw new Exception("Attempt not in context.");

            _dbcontext.UserDeviceChallengeAttempts.Update(attempt);
        }

        public async Task UpdateAttemptAsync(UserDeviceChallengeAttempt attempt)
        {
            if (!EfHelper.ExistsInContext(_dbcontext, attempt)) throw new Exception("Attempt not in context");

            _dbcontext.UserDeviceChallengeAttempts.Update(attempt);

            await _dbcontext.SaveChangesAsync();
        }

        public async Task<(bool isValid, UserDeviceChallengeAttempt? attempt)> ValidateAttemptAsync(string email, string code)
        {
            var validTime = _timeWindows.DeviceChallengeTime;

            var attempt = await _dbcontext.UserDeviceChallengeAttempts
                .Where(x => x.Email == email && x.Code == code && x.IsSuccessful == false).FirstOrDefaultAsync();

            if (attempt is null) return (false, null);

            if(!attempt.IsValid(validTime)) return (false, null);

            return (true, attempt);
        }
    }
}
