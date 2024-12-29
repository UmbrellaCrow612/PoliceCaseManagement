using Identity.Core.Models;
using Identity.Infrastructure.Data.Stores;
using Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Tests
{
    public class TokenStoreTests
    {
        private static IdentityApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<IdentityApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new IdentityApplicationDbContext(options);
        }

        private static TokenStore GetTokenStore(IdentityApplicationDbContext dbContext)
        {
            return new TokenStore(dbContext);
        }

        [Fact]
        public async Task StoreTokenAsync_ShouldAddTokenToDatabase()
        {
            // Arrange
            var dbContext = GetDbContext();
            var tokenStore = GetTokenStore(dbContext);

            var token = new Token
            {
                Id = Guid.NewGuid().ToString(),
                UserId = "user123",
                RefreshToken = "refreshToken123",
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7),
                UserDeviceId = "device123"
            };

            // Act
            await tokenStore.StoreTokenAsync(token);

            // Assert
            var storedToken = await dbContext.Tokens.FirstOrDefaultAsync(x => x.Id == token.Id);
            Assert.NotNull(storedToken);
            Assert.Equal(token.Id, storedToken.Id);
        }

        [Fact]
        public async Task SetToken_ShouldAddToContextWithoutSaving()
        {
            // Arrange
            var dbContext = GetDbContext();
            var tokenStore = GetTokenStore(dbContext);

            var token = new Token
            {
                Id = Guid.NewGuid().ToString(),
                UserId = "user123",
                RefreshToken = "refreshToken123",
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7),
                UserDeviceId = "device123"
            };

            // Act
            await tokenStore.SetToken(token);

            // Assert
            var storedToken = await dbContext.Tokens.FirstOrDefaultAsync(x => x.Id == token.Id);
            Assert.Null(storedToken); // Should not be in database yet

            var entry = dbContext.Entry(token);
            Assert.Equal(EntityState.Added, entry.State); // Should be marked for addition
        }

        [Fact]
        public async Task CleanupExpiredTokensAsync_ShouldRemoveExpiredTokens()
        {
            // Arrange
            var dbContext = GetDbContext();
            var tokenStore = GetTokenStore(dbContext);

            var expiredToken = new Token
            {
                Id = Guid.NewGuid().ToString(),
                UserId = "user123",
                RefreshToken = "expiredToken",
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(-1), // Expired
                UserDeviceId = "device123"
            };

            var validToken = new Token
            {
                Id = Guid.NewGuid().ToString(),
                UserId = "user456",
                RefreshToken = "validToken",
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7), // Not expired
                UserDeviceId = "device456"
            };

            dbContext.Tokens.Add(expiredToken);
            dbContext.Tokens.Add(validToken);
            await dbContext.SaveChangesAsync();

            // Act
            var removedCount = await tokenStore.CleanupExpiredTokensAsync();

            // Assert
            Assert.Equal(1, removedCount);
            var tokensInDb = await dbContext.Tokens.ToListAsync();
            Assert.Single(tokensInDb);
            Assert.Equal(validToken.Id, tokensInDb.First().Id);
        }

        [Fact]
        public async Task RevokeTokenAsync_ShouldRevokeAndBlacklistToken()
        {
            // Arrange
            var dbContext = GetDbContext();
            var tokenStore = GetTokenStore(dbContext);

            var token = new Token
            {
                Id = Guid.NewGuid().ToString(),
                UserId = "user123",
                RefreshToken = "refreshToken123",
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7),
                UserDeviceId = "device123"
            };

            dbContext.Tokens.Add(token);
            await dbContext.SaveChangesAsync();

            // Act
            await tokenStore.RevokeTokenAsync(token.Id);

            // Assert
            var revokedToken = await dbContext.Tokens.FirstOrDefaultAsync(x => x.Id == token.Id);
            Assert.NotNull(revokedToken);
            Assert.True(revokedToken.IsRevoked);
            Assert.True(revokedToken.IsBlackListed);
        }

        [Fact]
        public async Task RevokeTokenAsync_ShouldNotThrowException_WhenTokenDoesNotExist()
        {
            // Arrange
            var dbContext = GetDbContext();
            var tokenStore = GetTokenStore(dbContext);

            // Act & Assert
            await tokenStore.RevokeTokenAsync(Guid.NewGuid().ToString());
            // Should not throw exception
        }

        [Fact]
        public async Task ValidateTokenAsync_ShouldReturnTrue_WhenTokenIsValid()
        {
            // Arrange
            var dbContext = GetDbContext();
            var tokenStore = GetTokenStore(dbContext);

            var token = new Token
            {
                Id = Guid.NewGuid().ToString(),
                UserId = "user123",
                RefreshToken = "refreshToken123",
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7),
                UserDeviceId = "device123"
            };

            dbContext.Tokens.Add(token);
            await dbContext.SaveChangesAsync();

            // Act
            var (isValid, expiresAt) = await tokenStore.ValidateTokenAsync(token.Id, token.UserDeviceId, token.RefreshToken);

            // Assert
            Assert.True(isValid);
            Assert.Equal(token.RefreshTokenExpiresAt, expiresAt);
        }

        [Fact]
        public async Task ValidateTokenAsync_ShouldReturnFalse_WhenTokenDoesNotExist()
        {
            // Arrange
            var dbContext = GetDbContext();
            var tokenStore = GetTokenStore(dbContext);

            // Act
            var (isValid, expiresAt) = await tokenStore.ValidateTokenAsync(
                Guid.NewGuid().ToString(), "device123", "refreshToken123");

            // Assert
            Assert.False(isValid);
            Assert.Null(expiresAt);
        }

        [Fact]
        public async Task ValidateTokenAsync_ShouldReturnFalse_WhenDeviceIdDoesNotMatch()
        {
            // Arrange
            var dbContext = GetDbContext();
            var tokenStore = GetTokenStore(dbContext);

            var token = new Token
            {
                Id = Guid.NewGuid().ToString(),
                UserId = "user123",
                RefreshToken = "refreshToken123",
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7),
                UserDeviceId = "device123"
            };

            dbContext.Tokens.Add(token);
            await dbContext.SaveChangesAsync();

            // Act
            var (isValid, expiresAt) = await tokenStore.ValidateTokenAsync(
                token.Id, "wrongDevice", token.RefreshToken);

            // Assert
            Assert.False(isValid);
            Assert.Null(expiresAt);
        }

        [Fact]
        public async Task ValidateTokenAsync_ShouldReturnFalse_WhenRefreshTokenDoesNotMatch()
        {
            // Arrange
            var dbContext = GetDbContext();
            var tokenStore = GetTokenStore(dbContext);

            var token = new Token
            {
                Id = Guid.NewGuid().ToString(),
                UserId = "user123",
                RefreshToken = "refreshToken123",
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7),
                UserDeviceId = "device123"
            };

            dbContext.Tokens.Add(token);
            await dbContext.SaveChangesAsync();

            // Act
            var (isValid, expiresAt) = await tokenStore.ValidateTokenAsync(
                token.Id, token.UserDeviceId, "wrongRefreshToken");

            // Assert
            Assert.False(isValid);
            Assert.Null(expiresAt);
        }

        [Fact]
        public async Task RevokeAllUserTokensAsync_ShouldRevokeAllNonBlacklistedTokensForUser()
        {
            // Arrange
            var dbContext = GetDbContext();
            var tokenStore = GetTokenStore(dbContext);

            var tokens = new List<Token>
        {
            new() {
                Id = Guid.NewGuid().ToString(),
                UserId = "user123",
                RefreshToken = "token1",
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7),
                UserDeviceId = "device1",
                IsBlackListed = false
            },
            new() {
                Id = Guid.NewGuid().ToString(),
                UserId = "user123",
                RefreshToken = "token2",
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7),
                UserDeviceId = "device2",
                IsBlackListed = true // Already blacklisted
            },
            new() {
                Id = Guid.NewGuid().ToString(),
                UserId = "user456", // Different user
                RefreshToken = "token3",
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7),
                UserDeviceId = "device3",
                IsBlackListed = false
            }
        };

            dbContext.Tokens.AddRange(tokens);
            await dbContext.SaveChangesAsync();

            // Act
            await tokenStore.RevokeAllUserTokensAsync("user123");

            // Assert
            var user123Tokens = await dbContext.Tokens
                .Where(x => x.UserId == "user123")
                .ToListAsync();
            var user456Tokens = await dbContext.Tokens
                .Where(x => x.UserId == "user456")
                .ToListAsync();

            Assert.True(user123Tokens[0].IsRevoked);
            Assert.True(user123Tokens[0].IsBlackListed);
            Assert.False(user123Tokens[1].IsRevoked); // Already blacklisted token should remain unchanged
            Assert.False(user456Tokens[0].IsRevoked); // Other user's token should remain unchanged
            Assert.False(user456Tokens[0].IsBlackListed);
        }

        [Fact]
        public async Task Tokens_ShouldReturnQueryableOfTokens()
        {
            // Arrange
            var dbContext = GetDbContext();
            var tokenStore = GetTokenStore(dbContext);

            var tokens = new List<Token>
        {
            new() {
                Id = Guid.NewGuid().ToString(),
                UserId = "user123",
                RefreshToken = "token1",
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7),
                UserDeviceId = "device1"
            },
            new() {
                Id = Guid.NewGuid().ToString(),
                UserId = "user456",
                RefreshToken = "token2",
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7),
                UserDeviceId = "device2"
            }
        };

            dbContext.Tokens.AddRange(tokens);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await tokenStore.Tokens
                .Where(t => t.UserId == "user123")
                .ToListAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("user123", result[0].UserId);
        }
    }
}
