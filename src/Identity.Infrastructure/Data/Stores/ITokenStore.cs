using Identity.Infrastructure.Data.Models;

namespace Identity.Infrastructure.Data.Stores
{
    public interface ITokenStore
    {
        IQueryable<Token> Tokens { get; }

        Task StoreTokenAsync(Token token);

        Task RevokeTokenAsync(Token token);

        /// <summary>
        /// Revokes user tokens - dose not call save changes - waits for another store call or save changes call.
        /// </summary>
        Task SetRevokeAllUserTokensAsync(ApplicationUser user);

        Task RevokeAllUserTokensAsync(string userId);

        Task<(bool isValid, DateTime? RefreshTokenExpiresAt, IEnumerable<string> Errors)> ValidateTokenAsync(string tokenId, string refreshToken);

        Task<int> CleanupExpiredTokensAsync();

        Task<Token?> GetTokenById(string tokenId);
    }
}
