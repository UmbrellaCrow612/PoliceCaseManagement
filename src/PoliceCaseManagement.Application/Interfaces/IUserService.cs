using PoliceCaseManagement.Application.DTOs.Users;

namespace PoliceCaseManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto user);

        /// <summary>
        /// Link a user to a <see cref="IEnumerable{string}"/> of roles
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="roles">The name of the roles.</param>
        Task LinkUserToRoles(string userId, IEnumerable<string> roles);

        /// <summary>
        /// Un-Link a user of <see cref="IEnumerable{string}"/> of roles
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="roles">The name of the roles.</param>
        Task UnLinkUserFromRoles(string userId, IEnumerable<string> roles);

        /// <summary>
        /// Get all roles a user has by thier ID.
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>List of roles.</returns>
        Task<IEnumerable<string>> GetUserRolesAsync(string userId);

        /// <summary>
        /// Get a user by there ID
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The dto user or null if they do not exist.</returns>
        Task<UserDto?> GetUserByIdAsync(string userId);

        Task<bool> UpdateUserByIdAsync(string userId, UpdateUserDto request);

        /// <summary>
        /// Delete a user by there ID
        /// </summary>
        /// <param name="userIdToDelete">The ID of the user who is going to be deleted.</param>
        /// <param name="userIdOfDeleter">The ID of the user who is deleting the user.</param>
        Task DeleteUserByIdAsync(string userIdToDelete, string userIdOfDeleter);
    }
}
