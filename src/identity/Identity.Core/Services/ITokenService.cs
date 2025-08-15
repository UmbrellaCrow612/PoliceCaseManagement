using Identity.Core.Models;
using Identity.Core.ValueObjects;

namespace Identity.Core.Services
{
    /// <summary>
    /// Business contract to interact with tokens / token in the system
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Issues of a set of JWT and refresh tokens for a given user - dose not call save changes but adds the tokens to the ef core context 
        /// </summary>
        /// <param name="user">The user to issue the token for</param>
        /// <param name="device">>The device to issue it to</param>
        /// <returns>Set of tokens which contain the JWT and refresh token values</returns>
        Task<Tokens> IssueTokens(ApplicationUser user, Device device);
    }
}
