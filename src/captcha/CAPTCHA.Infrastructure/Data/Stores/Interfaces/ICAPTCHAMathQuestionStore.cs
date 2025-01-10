using CAPTCHA.Core.Models;

namespace CAPTCHA.Infrastructure.Data.Stores.Interfaces
{
    public interface ICAPTCHAMathQuestionStore
    {
        Task AddAsync(CAPTCHAMathQuestion question);

        Task<CAPTCHAMathQuestion?> FindByIdAsync(string captchaMathQuestionId);

        Task<bool> ExistsAsync(string captchaMathQuestionId);

        Task UpdateAsync(CAPTCHAMathQuestion question);
    }
}
