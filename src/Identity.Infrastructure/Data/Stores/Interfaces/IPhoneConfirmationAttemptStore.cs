using Identity.Core.Models;
using Shared.DTOs;

namespace Identity.Infrastructure.Data.Stores.Interfaces
{
    public interface IPhoneConfirmationAttemptStore
    {
        IQueryable<PhoneConfirmationAttempt> PhoneConfirmationAttempts { get; }

        Task<(bool canMakeAttempt, ICollection<ErrorDetail> errors)> AddAttempt(PhoneConfirmationAttempt attempt);
        Task<(bool isValid, PhoneConfirmationAttempt? attempt, ICollection<ErrorDetail> errors)> ValidateAttempt(string phoneNumber, string code);
        Task UpdateAsync(PhoneConfirmationAttempt attempt);
        void SetToUpdate(PhoneConfirmationAttempt attempt);
    }
}
