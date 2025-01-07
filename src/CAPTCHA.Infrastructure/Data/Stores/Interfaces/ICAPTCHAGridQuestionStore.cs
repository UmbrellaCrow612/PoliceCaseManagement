using CAPTCHA.Core.Models;

namespace CAPTCHA.Infrastructure.Data.Stores.Interfaces
{
    public interface ICAPTCHAGridQuestionStore
    {
        Task AddAsync(CAPTCHAGridParentQuestion question);

        Task<CAPTCHAGridParentQuestion?> FindByIdAsync(string captchaGridParentQuestionId);

        Task<bool> ExistsAsync(string captchaGridParentQuestionId);

        Task UpdateAsync(CAPTCHAGridParentQuestion question);
    }
}
