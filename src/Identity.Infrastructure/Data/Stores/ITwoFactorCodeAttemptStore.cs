using Identity.Core.Models;
using Shared.DTOs;

namespace Identity.Infrastructure.Data.Stores
{
    public interface ITwoFactorCodeAttemptStore
    {
        IQueryable<TwoFactorCodeAttempt> TwoFactorCodeAttempts { get; }

        Task<(bool canMakeAttempt, ICollection<ErrorDetail> errors)> AddAttempt(TwoFactorCodeAttempt attempt);
        Task<(bool isValid, TwoFactorCodeAttempt? attempt, ApplicationUser? user, ICollection<ErrorDetail> errors)> ValidateAttempt(string loginAttemptId, string code);
        void SetToUpdateAttempt(TwoFactorCodeAttempt attempt);
    }
}
