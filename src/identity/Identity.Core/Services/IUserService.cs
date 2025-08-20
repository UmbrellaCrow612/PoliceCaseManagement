using Identity.Core.Models;
using Results.Abstractions;
using Pagination.Abstractions;
using Identity.Core.ValueObjects;

namespace Identity.Core.Services
{
    /// <summary>
    /// Business contract to interact with users
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Find a user by there ID
        /// </summary>
        /// <param name="userId">The ID of the user to fetch</param>
        Task<ApplicationUser?> FindByIdAsync(string userId);

        /// <summary>
        /// Find a user by there email
        /// </summary>
        /// <param name="email">The email to search a user with</param>
        Task<ApplicationUser?> FindByEmailAsync(string email);

        /// <summary>
        /// Find a user by there phone number
        /// </summary>
        /// <param name="phoneNumber">The phone number verified</param>
        Task<ApplicationUser?> FindByPhoneNumberAsync(string phoneNumber);

        /// <summary>
        /// Checks if a user exists
        /// </summary>
        /// <param name="userId">The ID of the user to check</param>
        Task<bool> ExistsAsync(string userId);

        /// <summary>
        /// Checks if a username is taken
        /// </summary>
        /// <param name="username">The username to check</param>
        Task<bool> IsUsernameTakenAsync(string username);

        /// <summary>
        /// Checks is email is taken
        /// </summary>
        /// <param name="email">The email to check</param>
        Task<bool> IsEmailTakenAsync(string email);

        /// <summary>
        /// Checks if a phone number is taken
        /// </summary>
        /// <param name="phoneNumber">Phone number to check</param>
        Task<bool> IsPhoneNumberTakenAsync(string phoneNumber);

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="user">The user to update with updated fields</param>
        Task<IResult> UpdateAsync(ApplicationUser user);

        /// <summary>
        /// Search for a list of users
        /// </summary>
        /// <param name="query">Query containing the field you want to search for and by</param>
        Task<PaginatedResult<ApplicationUser>> SearchAsync(SearchUserQuery query);

        /// <summary>
        /// Create a user and set there password
        /// </summary>
        /// <param name="user">The user to create</param>
        /// <param name="password">Plain text password to set for the user</param>
        Task<IResult> CreateAsync(ApplicationUser user, string password);

        /// <summary>
        /// Checks a plain check password against the user's hashed password
        /// </summary>
        /// <param name="user">The user to check the password for</param>
        /// <param name="password">The password to check against the users</param>
        bool CheckPassword(ApplicationUser user, string password);
    }
}
