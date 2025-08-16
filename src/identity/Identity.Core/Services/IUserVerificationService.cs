using Identity.Core.Models;
using Results.Abstractions;

namespace Identity.Core.Services
{
    /// <summary>
    /// Business contract to handle all user email, phone number etc verification logic
    /// </summary>
    public interface IUserVerificationService
    {
        /// <summary>
        /// Send a email verification attempt for a given user
        /// </summary>
        /// <param name="user">The user who's email you want to verify</param>
        Task<UserVerificationResult> SendEmailVerification(ApplicationUser user);

        /// <summary>
        /// Verifies a email verification code for a given user
        /// </summary>
        /// <param name="user">The user who to verify</param>
        /// <param name="code">The code that was sent</param>
        Task<UserVerificationResult> VerifyEmail(ApplicationUser user, string code);

        /// <summary>
        /// Send a SMS phone verification code for the given user to verify there phone number is valid
        /// </summary>
        /// <param name="user">TRhe user who's phone number to verify</param>
        Task<UserVerificationResult> SendPhoneVerification(ApplicationUser user);

        /// <summary>
        /// Verifies a user's phone number with the SMS code sent to them
        /// </summary>
        /// <param name="user">The user who's phone number you want to verify</param>
        /// <param name="code">The code that was sent to them</param>
        Task<UserVerificationResult> VerifyPhoneNumber(ApplicationUser user, string code);
    }

    public class UserVerificationError : IResultError
    {
        public required string Code { get; set; }
        public required string? Message { get; set; }
    }

    /// <summary>
    /// Base result object for <see cref="IUserVerificationService"/> operations 
    /// </summary>
    public class UserVerificationResult : IResult
    {
        public bool Succeeded { get; set; } = false;
        public ICollection<IResultError> Errors { get; set; } = [];

        public void AddError(string code, string? message = null)
        {
            Errors.Add(new UserVerificationError { Code = code, Message = message});
        }
    }
}
