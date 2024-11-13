using Identity.Infrastructure.Data.Models;

namespace Identity.Infrastructure.Data.Stores
{
    public interface IPasswordResetAttemptStore
    {
        /// <returns>
        /// Checks if the user can make a password reset attempt (i.e., they haven't made a request in the last 30 minutes) and if so, adds the attempt to the database.
        /// </returns>
        Task<(bool canMakeAttempt, bool successfullyAdded)> AddAttempt(PasswordResetAttempt attempt);

        /// <summary>
        /// Revokes all password reset attempts for the specified user that are currently not successful, within the time limit, and not currently revoked.
        /// </summary>
        Task<bool> RevokePasswordAttempt(string userId);
    }
}
