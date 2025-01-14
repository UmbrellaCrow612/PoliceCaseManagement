using Identity.Core.Models;

namespace Identity.Infrastructure.Data.Stores.Interfaces
{
    public interface ITwoFactorSmsAttemptStore
    {
        IQueryable<TwoFactorSmsAttempt> TwoFactorSmsAttempts { get; }

        Task AddAsync(TwoFactorSmsAttempt attempt);

        Task<TwoFactorSmsAttempt?> FindAsync(string loginAttemptId, string code);

        void SetToUpdateAttempt(TwoFactorSmsAttempt attempt);
    }
}
