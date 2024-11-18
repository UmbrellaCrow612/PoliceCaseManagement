using Identity.Infrastructure.Data.Models;

namespace Identity.Infrastructure.Data.Stores
{
    public interface ITokenStore
    {
        Task StoreTokenAsync(Token token);

        Task RevokeTokenAsync(string tokenId);

        Task RevokeAllUserTokensAsync(string userId);

        Task<TokenValidationResult> ValidateTokenAsync(string tokenId, string refreshToken);

        Task<int> CleanupExpiredTokensAsync();

        Task SetDeviceInfo(DeviceInfo info);

        Task StoreLoginAttempt(LoginAttempt loginAttempt);

        Task SetLoginAttempt(LoginAttempt loginAttempt);
    }

    public class TokenValidationResult
    {
        public bool Succeeded { get; set; }

        public DateTime RefreshTokenExpiresAt { get; set; }
    }
}
