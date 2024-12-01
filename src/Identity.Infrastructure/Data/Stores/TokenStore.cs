
using Identity.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Identity.Infrastructure.Data.Stores
{
    public class TokenStore(IdentityApplicationDbContext dbContext, IConfiguration configuration) : ITokenStore
    {
        private readonly IdentityApplicationDbContext _dbContext = dbContext;
        private readonly IConfiguration _configuration = configuration;

        public IQueryable<Token> Tokens => _dbContext.Tokens.AsNoTracking();

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

        public async Task<(bool isValid, DateTime? RefreshTokenExpiresAt, IEnumerable<string> Errors)> ValidateTokenAsync(string tokenId, string refreshToken)
        {
            List<string> errors = [];
            double lifetime =  double.Parse(_configuration["Jwt:ExpiresInMinutes"] ?? throw new ApplicationException("Jwt:ExpiresInMinutes not provided"));
            double halfLifeTime = lifetime / 2;

            var token = await _dbContext.Tokens.FirstOrDefaultAsync(x => x.Id == tokenId);
            if(
                token is null 
                || token.IsRevoked 
                || token.IsBlackListed 
                || token.RefreshToken != refreshToken
                || token.RefreshTokenExpiresAt < DateTime.UtcNow
                )
            {
                errors.Add("Token Invalid");
                return (false, null, errors);
            }

            if (DateTime.UtcNow < token.CreatedAt.AddMinutes(halfLifeTime))
            {
                errors.Add("Token is still valid for over half its lifetime, please request after the half life period.");
                return (false, null, errors);
            }

            return (true, token.RefreshTokenExpiresAt, errors);
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
