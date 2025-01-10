using Identity.Core.Models;
using Shared.DTOs;

namespace Identity.Infrastructure.Data.Stores.Interfaces
{
    public interface ITwoFactorEmailAttemptStore
    {
        public IQueryable<TwoFactorEmailAttempt> TwoFactorEmailAttempts { get; }

        public Task<(bool canMakeAttempt, List<ErrorDetail> errors)> AddAttempt(TwoFactorEmailAttempt attempt);

        public Task<(bool isValid, List<ErrorDetail> errors)> ValidateAttempt(string loginAttemptId, string code);

    }
}
