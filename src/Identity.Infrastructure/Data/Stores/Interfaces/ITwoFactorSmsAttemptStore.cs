using Identity.Core.Models;
using Shared.DTOs;

namespace Identity.Infrastructure.Data.Stores.Interfaces
{
    public interface ITwoFactorSmsAttemptStore
    {
        IQueryable<TwoFactorSmsAttempt> TwoFactorSmsAttempts { get; }

        Task<(bool canMakeAttempt, ICollection<ErrorDetail> errors)> AddAttempt(TwoFactorSmsAttempt attempt);
        Task<(bool isValid, TwoFactorSmsAttempt? attempt, ApplicationUser? user, ICollection<ErrorDetail> errors)> ValidateAttempt(string loginAttemptId, string code);
        void SetToUpdateAttempt(TwoFactorSmsAttempt attempt);
    }
}
