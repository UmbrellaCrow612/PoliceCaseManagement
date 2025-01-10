using CAPTCHA.Core.Models;

namespace CAPTCHA.Infrastructure.Data.Stores.Interfaces
{
    public interface ICAPTCHAGridQuestionStore
    {
        Task AddAsync(CAPTCHAGridQuestion question, ICollection<CAPTCHAGridChild> gridChildren);

        Task<CAPTCHAGridQuestion?> FindByIdAsync(string captchaGridParentQuestionId);

        Task<bool> ExistsAsync(string captchaGridParentQuestionId);

        Task UpdateAsync(CAPTCHAGridQuestion question);
    }
}
