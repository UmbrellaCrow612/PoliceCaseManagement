using CAPTCHA.Core.Models;
using CAPTCHA.Infrastructure.Data.Stores.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CAPTCHA.Infrastructure.Data.Stores
{
    internal class CAPTCHAGridQuestionStore(CAPTCHAApplicationDbContext dbContext) : ICAPTCHAGridQuestionStore
    {
        private readonly CAPTCHAApplicationDbContext _dbContext = dbContext;

        public async Task AddAsync(CAPTCHAGridParentQuestion question)
        {
            await _dbContext.CAPTCHAGridParentQuestions.AddAsync(question);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(string captchaGridParentQuestionId)
        {
            return await _dbContext.CAPTCHAGridParentQuestions.AnyAsync(x => x.Id == captchaGridParentQuestionId);
        }

        public async Task<CAPTCHAGridParentQuestion?> FindByIdAsync(string captchaGridParentQuestionId)
        {
            return await _dbContext.CAPTCHAGridParentQuestions.Include(x => x.Children).FirstOrDefaultAsync(x => x.Id == captchaGridParentQuestionId);
        }

        public async Task UpdateAsync(CAPTCHAGridParentQuestion question)
        {
            _dbContext.CAPTCHAGridParentQuestions.Update(question);
            await _dbContext.SaveChangesAsync();
        }
    }
}
