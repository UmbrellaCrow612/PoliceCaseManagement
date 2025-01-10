using CAPTCHA.Core.Models;
using CAPTCHA.Infrastructure.Data.Stores.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CAPTCHA.Infrastructure.Data.Stores
{
    internal class CAPTCHAMathQuestionStore(CAPTCHAApplicationDbContext dbContext) : ICAPTCHAMathQuestionStore
    {
        private readonly CAPTCHAApplicationDbContext _dbContext = dbContext;

        public async Task AddAsync(CAPTCHAMathQuestion question)
        {
            await _dbContext.CAPTCHAMathQuestions.AddAsync(question);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(string captchaMathQuestionId)
        {
            return await _dbContext.CAPTCHAMathQuestions.AnyAsync(x => x.Id == captchaMathQuestionId);
        }

        public async Task<CAPTCHAMathQuestion?> FindByIdAsync(string captchaMathQuestionId)
        {
            return await _dbContext.CAPTCHAMathQuestions.FindAsync(captchaMathQuestionId);
        }

        public async Task UpdateAsync(CAPTCHAMathQuestion question)
        {
            _dbContext.Update(question);
            await _dbContext.SaveChangesAsync();
        }
    }
}
