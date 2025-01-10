using Identity.Core.Models;
using Utils.DTOs;

namespace Identity.Infrastructure.Data.Stores.Interfaces
{
    public interface ITwoFactorSmsAttemptStore
    {
        IQueryable<TwoFactorSmsAttempt> TwoFactorSmsAttempts { get; }

        Task<(bool canMakeAttempt, ICollection<ErrorDetail> errors)> AddAttempt(TwoFactorSmsAttempt attempt);
        Task<(bool isValid, ICollection<ErrorDetail> errors)> ValidateAttempt(string loginAttemptId, string code);
        void SetToUpdateAttempt(TwoFactorSmsAttempt attempt);
    }
}
