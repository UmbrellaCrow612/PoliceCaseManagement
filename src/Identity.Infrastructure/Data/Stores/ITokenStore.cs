namespace Identity.Infrastructure.Data.Stores
{
    public interface ITokenStore
    {
        Task StoreTokenAsync();

        Task RevokeTokenAsync();

        Task RevokeAllUserTokensAsync();

        Task<TokenValidationResult> ValidateTokenAsync();

        Task<int> CleanupExpiredTokensAsync();
    }

    public class TokenValidationResult
    {

    }
}
