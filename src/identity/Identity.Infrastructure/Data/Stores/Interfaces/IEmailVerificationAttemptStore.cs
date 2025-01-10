using Identity.Core.Models;
using Utils.DTOs;

namespace Identity.Infrastructure.Data.Stores.Interfaces
{
    public interface IEmailVerificationAttemptStore
    {
        Task<(bool canMakeAttempt, ICollection<ErrorDetail> errors)> AddAttemptAsync(EmailVerificationAttempt attempt);
        Task<(bool isValid, EmailVerificationAttempt? attempt, ApplicationUser? user, ICollection<string> errors)> ValidateAttemptAsync(string email, string code);
        void SetToUpdateAttempt(EmailVerificationAttempt attempt);
    }
}
