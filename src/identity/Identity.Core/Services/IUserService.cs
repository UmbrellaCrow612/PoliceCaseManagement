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
        /// Checks if a user exists
        /// </summary>
        /// <param name="userId">The ID of the user to check</param>
        Task<bool> ExistsAsync(string userId);

        /// <summary>
        /// Checks if a username is taken
        /// </summary>
        /// <param name="username">The username to check</param>
        Task<bool> IsUsernameTaken(string username);

        /// <summary>
        /// Checks is email is taken
        /// </summary>
        /// <param name="email">The email to check</param>
        Task<bool> IsEmailTaken(string email);

        /// <summary>
        /// Checks if a phone number is taken
        /// </summary>
        /// <param name="phoneNumber">Phone number to check</param>
        Task<bool> IsPhoneNumberTaken(string phoneNumber);

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="user">The user to update with updated fields</param>
        Task<UserServiceResult> UpdateAsync(ApplicationUser user);

        /// <summary>
        /// Search for a list of users
        /// </summary>
        /// <param name="query">Query containing the field you want to search for and by</param>
        Task<PaginatedResult<ApplicationUser>> SearchAsync(SearchUserQuery query);
    }

    public class UserServiceError : IResultError
    {
        public required string Code { get; set; }
        public required string? Message { get; set; } = null;
    }

    /// <summary>
    /// Result object to use for <see cref="IUserService"/> methods that return a <see cref="IResult"/>
    /// </summary>
    public class UserServiceResult : IResult
    {
        public bool Succeeded { get; set; } = false;
        public ICollection<IResultError> Errors { get; set; } = [];

        public void AddError(string code, string? message = null)
        {
            Errors.Add(new UserServiceError { Code = code, Message = message });
        }
    }
}
