using Identity.Core.Models;
using Identity.Infrastructure.Data.Stores.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data.Stores
{
    public class TwoFactorSmsAttemptStore(IdentityApplicationDbContext dbContext) : ITwoFactorSmsAttemptStore
    {
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;

        public IQueryable<TwoFactorSmsAttempt> TwoFactorSmsAttempts => _dbcontext.TwoFactorSmsAttempts.AsQueryable();

        public async Task AddAsync(TwoFactorSmsAttempt attempt)
        {
            await _dbcontext.TwoFactorSmsAttempts.AddAsync(attempt);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<TwoFactorSmsAttempt?> FindAsync(string loginAttemptId, string code)
        {
            return await _dbcontext.TwoFactorSmsAttempts.Where(x => x.LoginAttemptId == loginAttemptId && x.Code == code).FirstOrDefaultAsync();
        }

        public void SetToUpdateAttempt(TwoFactorSmsAttempt attempt)
        {
            _dbcontext.TwoFactorSmsAttempts.Update(attempt);
        }
    }
}
