﻿using Identity.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Utils;

namespace Identity.Infrastructure.Data.Stores
{
    public class LoginAttemptStore(IdentityApplicationDbContext dbContext) : ILoginAttemptStore
    {
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;

        public IQueryable<LoginAttempt> LoginAttempts => _dbcontext.LoginAttempts.AsQueryable();

        public async Task<LoginAttempt?> GetLoginAttemptById(string loginAttemptId)
        {
            return await _dbcontext.LoginAttempts.FirstOrDefaultAsync(x => x.Id == loginAttemptId);
        }

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

        public void SetToUpdateAttempt(LoginAttempt loginAttempt)
        {
            _dbcontext.LoginAttempts.Update(loginAttempt);
        }

        public async Task StoreLoginAttempt(LoginAttempt loginAttempt)
        {
            if(!EfHelper.ExistsInContext(_dbcontext, loginAttempt))
            {
                await _dbcontext.LoginAttempts.AddAsync(loginAttempt);
            }

            await _dbcontext.SaveChangesAsync();
        }
    }
}
