using Identity.Core.Models;

namespace Identity.Core.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Find a user by there ID
        /// </summary>
        /// <param name="userId">The ID of the user to fetch</param>
        Task<ApplicationUser?> FindByIdAsync(string userId);

        /// <summary>
        /// Checks if a username is taken
        /// </summary>
        /// <param name="username">The username to check</param>
        Task<bool> IsUsernameTaken(string username);
    }
}
