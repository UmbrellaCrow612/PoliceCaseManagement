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

        Task<IEnumerable<string>> GetUserRolesAsync(string userId);
    }
}
