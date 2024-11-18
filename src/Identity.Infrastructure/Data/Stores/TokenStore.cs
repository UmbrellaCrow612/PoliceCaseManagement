
using Identity.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data.Stores
{
    public class TokenStore(IdentityApplicationDbContext dbContext) : ITokenStore
    {
        private readonly IdentityApplicationDbContext _dbContext = dbContext;

        public async Task SetDeviceInfo(DeviceInfo info)
        {
            await _dbContext.DeviceInfos.AddAsync(info);
        }

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
                token.IsRevoked = true;
                token.IsBlackListed = true;
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task RevokeTokenAsync(string tokenId)
        {
            var token = await _dbContext.Tokens.FirstOrDefaultAsync(x => x.Id == tokenId);

            if (token is not null)
            {
                token.IsRevoked = true;
                token.IsBlackListed = true;

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task StoreTokenAsync(Token token)
        {
            await _dbContext.Tokens.AddAsync(token);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<TokenValidationResult> ValidateTokenAsync(string tokenId, string refreshToken)
        {
            var result = new TokenValidationResult();

            var token = await _dbContext.Tokens.FirstOrDefaultAsync(x => x.Id == tokenId);
            if(
                token is null 
                || token.IsRevoked 
                || token.IsBlackListed 
                || token.RefreshToken != refreshToken
                || token.RefreshTokenExpiresAt < DateTime.UtcNow
                )
            {
                result.Succeeded = false;
                return result;
            }

            result.Succeeded = true;
            result.RefreshTokenExpiresAt = token.RefreshTokenExpiresAt;

            return result;
        }

        public async Task StoreLoginAttempt(LoginAttempt loginAttempt)
        {
            await _dbContext.LoginAttempts.AddAsync(loginAttempt);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SetLoginAttempt(LoginAttempt loginAttempt)
        {
            await _dbContext.LoginAttempts.AddAsync(loginAttempt);
        }
    }
}
