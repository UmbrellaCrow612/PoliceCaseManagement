using Identity.Core.Models;

namespace Identity.Infrastructure.Data.Stores.Interfaces
{
    public interface ITokenStore
    {
        IQueryable<Token> Tokens { get; }

        Task SetToken(Token token);
        Task StoreTokenAsync(Token token);

        Task RevokeTokenAsync(Token token);

        /// <summary>
        /// Revokes user tokens - dose not call save changes - waits for another store call or save changes call.
        /// </summary>
        Task SetRevokeAllUserTokensAsync(ApplicationUser user);

        Task RevokeAllUserTokensAsync(string userId);

        Task<(bool isValid, DateTime? RefreshTokenExpiresAt)> ValidateTokenAsync(string tokenId, string deviceId, string refreshToken);

        Task<int> CleanupExpiredTokensAsync();

        Task<Token?> GetTokenById(string tokenId);
    }
}
