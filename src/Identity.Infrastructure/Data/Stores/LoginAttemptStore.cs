using Identity.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data.Stores
{
    public class LoginAttemptStore(IdentityApplicationDbContext dbContext) : ILoginAttemptStore
    {
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;

        public IQueryable<LoginAttempt> LoginAttempts => _dbcontext.LoginAttempts.AsQueryable();

        public async Task<ICollection<LoginAttempt>> GetUserLoginAttempts(ApplicationUser user)
        {
            _ = _dbcontext.Users.Local.FirstOrDefault(x => x.Id == user.Id) ?? throw new ApplicationException("User not in context.");

            var loginAttempts = await _dbcontext.LoginAttempts.Where(x => x.UserId == user.Id).ToListAsync();

            return loginAttempts;
        }

        public async Task SetLoginAttempt(LoginAttempt loginAttempt)
        {
            await _dbcontext.LoginAttempts.AddAsync(loginAttempt);
        }

        public async Task StoreLoginAttempt(LoginAttempt loginAttempt)
        {
            await _dbcontext.LoginAttempts.AddAsync(loginAttempt);
            await _dbcontext.SaveChangesAsync();
        }
    }
}
