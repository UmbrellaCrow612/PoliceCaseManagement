using CAPTCHA.Core.Models;
using CAPTCHA.Infrastructure.Data.Stores.Interfaces;

namespace CAPTCHA.Infrastructure.Data.Stores
{
    internal class CAPTCHAGridChildStore(CAPTCHAApplicationDbContext dbContext) : ICAPTCHAGridChildStore
    {
        private readonly CAPTCHAApplicationDbContext _dbcontext = dbContext;

        public async Task AddAsync(CAPTCHAGridChild child)
        {
            await _dbcontext.CAPTCHAGridChildren.AddAsync(child);
            await _dbcontext.SaveChangesAsync();
            
        }

        public async Task AddManyAsync(ICollection<CAPTCHAGridChild> children)
        {
            await _dbcontext.CAPTCHAGridChildren.AddRangeAsync(children);
            await _dbcontext.SaveChangesAsync();
        }

        public Task<bool> ExistsAsync(string captchaGridChildId)
        {
            throw new NotImplementedException();
        }

        public Task<CAPTCHAGridChild?> FindByIdAsync(string captchaGridChildId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(CAPTCHAGridChild question)
        {
            throw new NotImplementedException();
        }
    }
}
