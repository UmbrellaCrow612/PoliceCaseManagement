using CAPTCHA.Core.Models;

namespace CAPTCHA.Infrastructure.Data.Stores.Interfaces
{
    public interface ICAPTCHAGridChildStore
    {
        Task AddManyAsync(ICollection<CAPTCHAGridChild> children);

        Task AddAsync(CAPTCHAGridChild child);

        Task<CAPTCHAGridChild?> FindByIdAsync(string captchaGridChildId);

        Task<bool> ExistsAsync(string captchaGridChildId);

        Task UpdateAsync(CAPTCHAGridChild question);
    }
}
