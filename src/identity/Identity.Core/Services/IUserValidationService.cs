using Identity.Core.Models;
using Results.Abstractions;

namespace Identity.Core.Services
{
    /// <summary>
    /// Provides all validation logic for <see cref="Core.Models.ApplicationUser"/>
    /// </summary>
    public interface IUserValidationService
    {
        /// <summary>
        /// Validates a username against our rules
        /// </summary>
        /// <param name="username">The username to check</param>
        IResult ValidateUsername(string username);

        /// <summary>
        /// Validate a email against our rules
        /// </summary>
        /// <param name="email">The email to check</param>
        IResult ValidateEmail(string email);

        /// <summary>
        /// Validate a phone number against our rules
        /// </summary>
        /// <param name="phoneNumber">The phone number to check</param>
        IResult ValidatePhoneNumber(string phoneNumber);

        /// <summary>
        /// Validate a password against our rules
        /// </summary>
        /// <param name="password">The password to check</param>
        IResult ValidatePassword(string password);

        /// <summary>
        /// Runs all our validation logic against a user
        /// </summary>
        /// <param name="user">The user to check</param>
        IResult Validate(ApplicationUser user);
    }
}
