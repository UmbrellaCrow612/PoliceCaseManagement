using Identity.Core.ValueObjects;
using Results.Abstractions;

namespace Identity.Core.Services
{
    /// <summary>
    /// Defines the business contract for handling all Multi-Factor Authentication (MFA) operations.
    /// </summary>
    public interface IMfaService
    {
        /// <summary>
        /// Sends an SMS-based two-factor authentication code for a specified <see cref="Models.Login"/>.
        /// </summary>
        /// <param name="loginId">The ID of the login attempt.</param>
        /// <param name="deviceInfo">Information about the device making the request.</param>
        Task<MfaResult> SendMfaSmsAsync(string loginId, DeviceInfo deviceInfo);

        /// <summary>
        /// Verifies an SMS-based MFA attempt for a specified <see cref="Models.Login"/> using the code sent.
        /// </summary>
        /// <param name="loginId">The ID of the login attempt the code was sent for.</param>
        /// <param name="code">The code that was sent via SMS.</param>
        /// <param name="deviceInfo">Information about the device making the request.</param>
        Task<VerifiedMfaResult> VerifyMfaSmsAsync(string loginId, string code, DeviceInfo deviceInfo);

        /// <summary>
        /// Verifies a Time-based One-Time Password (TOTP) for a given login attempt.
        /// </summary>
        /// <param name="loginId">The ID of the login attempt.</param>
        /// <param name="code">The TOTP code to verify.</param>
        /// <param name="deviceInfo">Information about the device making the request.</param>
        Task<VerifiedMfaResult> VerifyTotpAsync(string loginId, string code, DeviceInfo deviceInfo);
    }

    /// <summary>
    /// Represents a base MFA service error.
    /// </summary>
    public class MfaError : IResultError
    {
        public required string Code { get; set; }
        public string? Message { get; set; } = null;
    }

    /// <summary>
    /// Represents the result of an MFA service operation.
    /// </summary>
    public class MfaResult : IResult
    {
        public bool Succeeded { get; set; } = false;

        public ICollection<IResultError> Errors { get; set; } = [];

        public void AddError(string code, string? message = null)
        {
            Errors.Add(new MfaError { Code = code, Message = message });
        }
    }

    /// <summary>
    /// Used when a operation produces session tokens
    /// </summary>
    public class VerifiedMfaResult : MfaResult
    {
        public Tokens Tokens { get; set; } = new Tokens();
    }
}
