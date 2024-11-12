
namespace Identity.Infrastructure.Data.Stores
{
    public class TokenStore(IdentityApplicationDbContext dbContext) : ITokenStore
    {
        private IdentityApplicationDbContext _dbContext = dbContext;

        public Task<int> CleanupExpiredTokensAsync()
        {
            throw new NotImplementedException();
        }

        public Task RevokeAllUserTokensAsync()
        {
            throw new NotImplementedException();
        }

        public Task RevokeTokenAsync()
        {
            throw new NotImplementedException();
        }

        public Task StoreTokenAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TokenValidationResult> ValidateTokenAsync()
        {
            throw new NotImplementedException();
        }
    }
}
