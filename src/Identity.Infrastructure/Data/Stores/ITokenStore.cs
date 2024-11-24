using Identity.Infrastructure.Data.Models;

namespace Identity.Infrastructure.Data.Stores
{
    public interface ITokenStore
    {
        IQueryable<Token> Tokens { get; }

        Task StoreTokenAsync(Token token);

        Task RevokeTokenAsync(Token token);

        Task RevokeAllUserTokensAsync(string userId);

        Task<(bool isValid, DateTime? RefreshTokenExpiresAt, IEnumerable<string> Errors)> ValidateTokenAsync(string tokenId, string refreshToken);

        Task<int> CleanupExpiredTokensAsync();

        Task SetDeviceInfo(DeviceInfo info);

        Task StoreLoginAttempt(LoginAttempt loginAttempt);

        Task SetLoginAttempt(LoginAttempt loginAttempt);

        Task<Token?> GetTokenById(string tokenId);
    }
}
