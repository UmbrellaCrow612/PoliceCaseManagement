using Identity.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data.Stores
{
    public class TokenStore(IdentityApplicationDbContext dbContext) : ITokenStore
    {
        private readonly IdentityApplicationDbContext _dbContext = dbContext;

        public IQueryable<Token> Tokens => _dbContext.Tokens.AsQueryable();

        public async Task<int> CleanupExpiredTokensAsync()
        {
            var tokens = await _dbContext.Tokens
                .Where(x => x.RefreshTokenExpiresAt < DateTime.UtcNow)
                .ToListAsync();

            var count = tokens.Count;

            _dbContext.Remove(tokens);

            await _dbContext.SaveChangesAsync();

            return count;

        }

        public async Task RevokeAllUserTokensAsync(string userId)
        {
            var tokens = await _dbContext.Tokens
                .Where(x => x.UserId == userId && x.IsRevoked == false && x.IsBlackListed == false)
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.Revoke();
                token.BlackList();
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task RevokeTokenAsync(string tokenId)
        {
            var token = await _dbContext.Tokens.FirstOrDefaultAsync(x => x.Id == tokenId);

            if (token is not null)
            {
                token.Revoke();
                token.BlackList();

                await _dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Adds it to context and waits for a save changes call to add it to db 
        /// </summary>
        public async Task SetToken(Token token)
        {
            await _dbContext.Tokens.AddAsync(token);
        }
        public async Task StoreTokenAsync(Token token)
        {
            await _dbContext.Tokens.AddAsync(token);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<(bool isValid, DateTime? RefreshTokenExpiresAt)> ValidateTokenAsync(string tokenId, string deviceId, string refreshToken)
        {
            var token = await _dbContext.Tokens.FirstOrDefaultAsync(x => x.Id == tokenId);

            if(token is null || !token.IsValid(refreshToken, deviceId))
            {
                return (false, null);
            }

            return (true, token.RefreshTokenExpiresAt);
        }

        public Task RevokeTokenAsync(Token token)
        {
            throw new NotImplementedException();
        }

        public Task<Token?> GetTokenById(string tokenId)
        {
            throw new NotImplementedException();
        }

        public Task SetRevokeAllUserTokensAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}
