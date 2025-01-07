using CAPTCHA.Core.Models;

namespace CAPTCHA.Infrastructure.Data.Stores.Interfaces
{
    public interface ICAPTCHAAudioQuestionStore
    {
        Task AddAsync(CAPTCHAAudioQuestion question);

        Task<CAPTCHAAudioQuestion?> FindByIdAsync(string captchaAudioQuestionId);

        Task<bool> ExistsAsync(string captchaAudioQuestionId);

        Task UpdateAsync(CAPTCHAAudioQuestion question);
    }
}
