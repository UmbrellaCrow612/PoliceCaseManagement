using Identity.Core.Models;
using Identity.Infrastructure.Data.Stores.Interfaces;
using Identity.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.DTOs;
using Shared.Utils;

namespace Identity.Infrastructure.Data.Stores
{
    public class PhoneConfirmationAttemptStore(IdentityApplicationDbContext dbContext, IOptions<TimeWindows> timeWindows) : IPhoneConfirmationAttemptStore
    {
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;
        private readonly TimeWindows _timeWindows = timeWindows.Value;

        public IQueryable<PhoneConfirmationAttempt> PhoneConfirmationAttempts => _dbcontext.PhoneConfirmationAttempts.AsQueryable();

        public async Task<(bool canMakeAttempt, ICollection<ErrorDetail> errors)> AddAttempt(PhoneConfirmationAttempt attempt)
        {
            List<ErrorDetail> errors = [];
            var validTime = _timeWindows.PhoneConfirmationTime;

            var validRecentAttempt = await _dbcontext.PhoneConfirmationAttempts
                .Where(x => 
                x.UserId == attempt.UserId &&
                x.IsSuccessful == false &&
                x.SuccessfulAt == null &&
                x.CreatedAt.AddMinutes(validTime) > DateTime.UtcNow)
                .OrderBy(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            if (validRecentAttempt is not null)
            {
                errors.Add(new ErrorDetail { Reason = "There is a valid recent phone confirmation attempt.", Field = "Phone confirmation attempt." });
                return (false, errors);
            }

            await _dbcontext.PhoneConfirmationAttempts.AddAsync(attempt);
            await _dbcontext.SaveChangesAsync();

            return (true, errors);
        }

        public void SetToUpdate(PhoneConfirmationAttempt attempt)
        {
            _dbcontext.PhoneConfirmationAttempts.Update(attempt);
        }

        public async Task UpdateAsync(PhoneConfirmationAttempt attempt)
        {
            if(EfHelper.ExistsInContext(_dbcontext, attempt))
            {
                _dbcontext.PhoneConfirmationAttempts.Update(attempt);
                await _dbcontext.SaveChangesAsync();
            }
        }

        public async Task<(bool isValid, PhoneConfirmationAttempt? attempt, ICollection<ErrorDetail> errors)> ValidateAttempt(string phoneNumber, string code)
        {
            List<ErrorDetail> errors = [];
            var validTime = _timeWindows.PhoneConfirmationTime;

            var _attempt = await _dbcontext.PhoneConfirmationAttempts.Where(x => x.PhoneNumber == phoneNumber && x.Code == code && x.IsSuccessful == false).FirstOrDefaultAsync();
            if(_attempt is null)
            {
                errors.Add(new ErrorDetail { Reason = "Phone confirmation not found for phone number and code.", Field = "Phone confirmation attempt." });
                return (false, null, errors);
            }

            if (!_attempt.IsValid(validTime))
            {
                errors.Add(new ErrorDetail { Reason = "Phone confirmation is invalid.", Field = "Phone confirmation attempt." });
                return (false, null, errors);
            }

            return (true, _attempt, errors);
        }
    }
}
