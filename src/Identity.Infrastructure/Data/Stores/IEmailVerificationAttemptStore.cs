using Identity.Infrastructure.Data.Models;

namespace Identity.Infrastructure.Data.Stores
{
    public interface IEmailVerificationAttemptStore
    {
        Task<(bool canMakeAttempt, bool successfullyAdded)> AddAttemptAsync(EmailVerificationAttempt attempt);
        Task<(bool isValid, EmailVerificationAttempt? attempt, ApplicationUser? user, ICollection<string> errors)> ValidateAttemptAsync(string email, string code);
        void SetToUpdateAttempt(EmailVerificationAttempt attempt);
    }
}
