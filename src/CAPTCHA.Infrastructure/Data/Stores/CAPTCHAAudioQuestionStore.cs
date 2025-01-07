using CAPTCHA.Core.Models;
using CAPTCHA.Infrastructure.Data.Stores.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CAPTCHA.Infrastructure.Data.Stores
{
    internal class CAPTCHAAudioQuestionStore(CAPTCHAApplicationDbContext dbContext) : ICAPTCHAAudioQuestionStore
    {
        private readonly CAPTCHAApplicationDbContext _dbContext = dbContext;

        public async Task AddAsync(CAPTCHAAudioQuestion question)
        {
            await _dbContext.CAPTCHAAudioQuestions.AddAsync(question);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(string captchaAudioQuestionId)
        {
            return await _dbContext.CAPTCHAAudioQuestions.AnyAsync(x => x.Id == captchaAudioQuestionId);
        }

        public async Task<CAPTCHAAudioQuestion?> FindByIdAsync(string captchaAudioQuestionId)
        {
            return await _dbContext.CAPTCHAAudioQuestions.FindAsync(captchaAudioQuestionId);
        }

        public async Task UpdateAsync(CAPTCHAAudioQuestion question)
        {
            _dbContext.CAPTCHAAudioQuestions.Update(question);
            await _dbContext.SaveChangesAsync();
        }
    }
}
