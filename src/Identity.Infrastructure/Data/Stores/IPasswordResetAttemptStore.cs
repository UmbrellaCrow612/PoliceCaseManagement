using Identity.Core.Models;
using Shared.DTOs;

namespace Identity.Infrastructure.Data.Stores
{
    public interface IPasswordResetAttemptStore
    {
        IQueryable<PasswordResetAttempt> PasswordResetAttempts { get; }

        /// <returns>
        /// Checks if the user can make a password reset attempt (i.e., they haven't made a request in the last 30 minutes) and if so, adds the attempt to the database.
        /// </returns>
        Task<(bool canMakeAttempt, ICollection<ErrorDetail> errors)> AddAttempt(PasswordResetAttempt attempt);

        /// <summary>
        /// Revokes all password reset attempts for the specified user that are currently not successful, within the time limit, and not currently revoked.
        /// </summary>
        Task<int> RevokeAllValidPasswordAttempts(string userId);

        Task<(bool isValid, PasswordResetAttempt? attempt)> ValidateAttempt(string code);

        /// <summary>
        /// Changes the entity in the context and waits for a save changes call
        /// </summary>
        void SetToUpdateAttempt(PasswordResetAttempt attempt);
    }
}
