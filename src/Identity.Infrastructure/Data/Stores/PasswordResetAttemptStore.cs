using Identity.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Identity.Infrastructure.Data.Stores
{
    public class PasswordResetAttemptStore(IConfiguration configuration, IdentityApplicationDbContext dbContext) : IPasswordResetAttemptStore
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;

        public async Task<(bool canMakeAttempt, bool successfullyAdded)> AddAttempt(PasswordResetAttempt attempt)
        {
            int resetPasswordSessionTimeInMinutes = int.Parse(_configuration["ResetPasswordSessionTimeInMinutes"] ?? throw new ApplicationException("ResetPasswordSessionTimeInMinutes not provided."));

            var recentAttempt = await _dbcontext.PasswordResetAttempts
                .Where(x => x.UserId == attempt.UserId
                 && x.IsSuccessful == false
                 && x.IsRevoked == false
                 && x.ValidSessionTime >= DateTime.UtcNow.AddMinutes(-resetPasswordSessionTimeInMinutes))
                .OrderByDescending(x => x.ValidSessionTime)
                .FirstOrDefaultAsync();

            if (recentAttempt is not null) return (false, false);

            await _dbcontext.PasswordResetAttempts.AddAsync(attempt);
            await _dbcontext.SaveChangesAsync();

            return (true, true);
        }

        public Task<bool> RevokePasswordAttempt(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
