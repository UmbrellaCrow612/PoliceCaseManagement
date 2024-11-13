using Identity.Infrastructure.Data.Models;

namespace Identity.Infrastructure.Data.Stores
{
    public interface ITokenStore
    {
        Task StoreTokenAsync(Token token);

        Task RevokeTokenAsync(string tokenId);

        Task RevokeAllUserTokensAsync(string userId);

        Task<TokenValidationResult> ValidateTokenAsync(string tokenId);

        Task<int> CleanupExpiredTokensAsync();
    }

    public class TokenValidationResult
    {
        public bool Success { get; set; }
    }
}
