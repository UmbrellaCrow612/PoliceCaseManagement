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
        Task<IResult> SendEmailVerification(ApplicationUser user);

        /// <summary>
        /// Verifies a email verification code for a given user
        /// </summary>
        /// <param name="user">The user who to verify</param>
        /// <param name="code">The code that was sent</param>
        Task<IResult> VerifyEmail(ApplicationUser user, string code);

        /// <summary>
        /// Send a SMS phone verification code for the given user to verify there phone number is valid
        /// </summary>
        /// <param name="user">TRhe user who's phone number to verify</param>
        Task<IResult> SendPhoneVerification(ApplicationUser user);

        /// <summary>
        /// Verifies a user's phone number with the SMS code sent to them
        /// </summary>
        /// <param name="user">The user who's phone number you want to verify</param>
        /// <param name="code">The code that was sent to them</param>
        Task<IResult> VerifyPhoneNumber(ApplicationUser user, string code);

        /// <summary>
        /// Verifies a TOTP (Time Based One Time Passcode) code is valid for the given user - based on there secret and checks if 
        /// the code sent would have been generated with the secret, if it is valid the user flag for confirmed TOTP will be turned on for use
        /// </summary>
        /// <param name="user">The user to check</param>
        /// <param name="code">The TOTP code</param>
        Task<IResult> VerifyTotp(ApplicationUser user, string code);
    }
}
