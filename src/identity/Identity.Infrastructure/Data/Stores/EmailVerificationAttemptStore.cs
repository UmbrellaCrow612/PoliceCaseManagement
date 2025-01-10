using Identity.Core.Models;
using Identity.Infrastructure.Data.Stores.Interfaces;
using Identity.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Utils.DTOs;

namespace Identity.Infrastructure.Data.Stores
{
    internal class EmailVerificationAttemptStore(IdentityApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IOptions<TimeWindows> timeWindows) : IEmailVerificationAttemptStore
    {
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly TimeWindows _timeWindows = timeWindows.Value;

        public async Task<(bool canMakeAttempt, ICollection<ErrorDetail> errors)> AddAttemptAsync(EmailVerificationAttempt attempt)
        {
            List<ErrorDetail> errors = [];

            var validTime = _timeWindows.EmailConfirmationTime;

            var recentValidAttempt = await _dbcontext.EmailVerificationAttempts
                .Where(x => 
                x.IsSuccessful == false && 
                x.CreatedAt.AddMinutes(validTime) > DateTime.UtcNow &&
                x.UsedAt == null &&
                x.UserId == attempt.UserId
                )
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            if (recentValidAttempt is not null)
            {
                errors.Add(new ErrorDetail
                {
                    Field = "Email confirmation.",
                    Reason = "There is at already a valid issued attempt."
                });
                return (false, errors);
            }

            await _dbcontext.EmailVerificationAttempts.AddAsync(attempt);
            await _dbcontext.SaveChangesAsync();

            return (true, errors);
        }

        public void SetToUpdateAttempt(EmailVerificationAttempt attempt)
        {
            _dbcontext.EmailVerificationAttempts.Update(attempt);
        }

        public async Task<(bool isValid, EmailVerificationAttempt? attempt, ApplicationUser? user, ICollection<string> errors)> ValidateAttemptAsync(string email, string code)
        {
            List<string> errors = [];

            var validTime = _timeWindows.EmailConfirmationTime;

            var user = await _userManager.FindByEmailAsync(email);
            if(user is null)
            {
                errors.Add("User dose not exist.");
                return (false, null, null, errors);
            }

            var _attempt = await _dbcontext.EmailVerificationAttempts
                .FirstOrDefaultAsync(x => x.Email == email && x.Code == code && x.IsSuccessful == false);
            if (_attempt is null)
            {
                errors.Add("Attempt not found.");
                return (false, null,null, errors);
            }

            if(!_attempt.IsValid(validTime))
            {
                errors.Add("Attempt invalid.");
                return (false, null, null, errors);
            }

            return (true, _attempt, user, errors);
        }
    }
}
