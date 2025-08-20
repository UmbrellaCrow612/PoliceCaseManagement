using Identity.Core.ValueObjects;
using Results.Abstractions;

namespace Identity.Core.Services
{
    /// <summary>
    /// Business contract for handling all user authentication operations.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Attempts to log in a user by validating their credentials and settings,
        /// then proceeds to the next step of 2FA if successful.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="deviceInfo">Information about the device being used.</param>
        /// <returns>
        /// A <see cref="LoginResult"/> object containing either the login ID for 2FA
        /// or error codes indicating the reason for failure.
        /// </returns>
        Task<LoginResult> LoginAsync(string email, string password, DeviceInfo deviceInfo);

        /// <summary>
        /// Logs out a user by removing all active sessions.
        /// </summary>
        /// <param name="userId">The user ID to log out.</param>
        /// <returns>A <see cref="IResult"/> object indicating success or failure.</returns>
        Task<IResult> LogoutAsync(string userId);

        /// <summary>
        /// Refreshes JWT tokens to issue a new set.
        /// </summary>
        /// <param name="refreshToken">The refresh token associated with the current JWT.</param>
        /// <param name="deviceInfo">Information about the device requesting the refresh.</param>
        /// <returns>A <see cref="IResult"/> object indicating success or failure as well as new set of jwt and refresh tokens on success.</returns>
        Task<RefreshResult> RefreshTokensAsync(string refreshToken, DeviceInfo deviceInfo);
    }

    public class LoginResult : Result
    {
        public string? LoginId { get; set; }
    }

    public class RefreshResult : Result
    {
        public Tokens Tokens { get; set; } = new Tokens { JwtBearerToken = "EMPTY", RefreshToken = "EMPTY" };
    }
}
