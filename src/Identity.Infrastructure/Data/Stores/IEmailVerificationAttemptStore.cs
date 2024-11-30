using Identity.Infrastructure.Data.Models;

namespace Identity.Infrastructure.Data.Stores
{
    public interface IEmailVerificationAttemptStore
    {
        Task<(bool canMakeAttempt, bool successfullyAdded)> AddAttempt(EmailVerificationAttempt attempt);
        void SetToUpdateAttempt(EmailVerificationAttempt attempt);
    }
}
