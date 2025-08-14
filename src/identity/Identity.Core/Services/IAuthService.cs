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
        Task<AuthResult> LogoutAsync(string userId);

        /// <summary>
        /// Refreshes JWT tokens to issue a new set.
        /// </summary>
        /// <param name="refreshToken">The refresh token associated with the current JWT.</param>
        /// <param name="deviceInfo">Information about the device requesting the refresh.</param>
        /// <returns>A <see cref="IResult"/> object indicating success or failure as well as new set of jwt and refresh tokens on success.</returns>
        Task<RefreshResult> RefreshTokensAsync(string refreshToken, DeviceInfo deviceInfo);
    }

    /// <summary>
    /// Base auth service error
    /// </summary>
    public class AuthError : IResultError
    {
        public required string Code { get; set; }
        public string? Message { get; set; } = null;
    }

    /// <summary>
    /// Base result object for auth service
    /// </summary>
    public class AuthResult : IResult
    {
        public bool Succeeded { get; set; } = false;

        public ICollection<IResultError> Errors { get; set; } = [];

        /// <param name="code">Could be a <see cref="Result.Codes"/></param>
        public void AddError(string code, string? message = null)
        {
            Errors.Add(new AuthError { Code = code, Message = message });
        }
    }

    /// <summary>
    /// Has the base <see cref="AuthResult"/> and also a <see cref="LoginId"/> of the <see cref="Models.Login"/>
    /// </summary>
    public class LoginResult : AuthResult
    {
        public string? LoginId { get; set; }
    }

    /// <summary>
    /// Has the base <see cref="AuthResult"/> and also set of tokens <see cref="ValueObjects.Tokens"/> of new JWT and refresh
    /// </summary>
    public class RefreshResult : AuthResult
    {
        public Tokens Tokens { get; set; } = new Tokens { JwtBearerToken = "EMPTY", RefreshToken = "EMPTY" };
    }
}
