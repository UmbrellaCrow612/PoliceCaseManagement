using Identity.Core.Models;

namespace Identity.Core.Tests
{
    public class TokenTests
    {
        [Fact]
        public void Constructor_InitializesDefaultProperties()
        {
            // Arrange & Act
            var token = new Token
            {
                Id = "test-id",
                UserId = "test-user",
                RefreshToken = "refresh-token",
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7),
                UserDeviceId = "device-id"
            };

            // Assert
            Assert.Null(token.User);
            Assert.False(token.IsRevoked);
            Assert.Null(token.RevokedAt);
            Assert.Null(token.RevokedReason);
            Assert.False(token.IsBlackListed);
            Assert.NotEqual(default(DateTime), token.CreatedAt);
        }

        [Fact]
        public void IsValid_WhenNewToken_ReturnsTrue()
        {
            // Arrange
            var token = new Token
            {
                Id = "test-id",
                UserId = "test-user",
                RefreshToken = "refresh-token",
                RefreshTokenExpiresAt = DateTime.UtcNow.AddHours(1),
                UserDeviceId = "device-id"
            };

            // Act
            bool result = token.IsValid("refresh-token", "device-id");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_WhenRevoked_ReturnsFalse()
        {
            // Arrange
            var token = new Token
            {
                Id = "test-id",
                UserId = "test-user",
                RefreshToken = "refresh-token",
                RefreshTokenExpiresAt = DateTime.UtcNow.AddHours(1),
                UserDeviceId = "device-id",
                IsRevoked = true
            };

            // Act
            bool result = token.IsValid("refresh-token", "device-id");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_WhenBlacklisted_ReturnsFalse()
        {
            // Arrange
            var token = new Token
            {
                Id = "test-id",
                UserId = "test-user",
                RefreshToken = "refresh-token",
                RefreshTokenExpiresAt = DateTime.UtcNow.AddHours(1),
                UserDeviceId = "device-id",
                IsBlackListed = true
            };

            // Act
            bool result = token.IsValid("refresh-token", "device-id");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_WhenIncorrectRefreshToken_ReturnsFalse()
        {
            // Arrange
            var token = new Token
            {
                Id = "test-id",
                UserId = "test-user",
                RefreshToken = "correct-refresh-token",
                RefreshTokenExpiresAt = DateTime.UtcNow.AddHours(1),
                UserDeviceId = "device-id"
            };

            // Act
            bool result = token.IsValid("wrong-refresh-token", "device-id");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_WhenIncorrectDeviceId_ReturnsFalse()
        {
            // Arrange
            var token = new Token
            {
                Id = "test-id",
                UserId = "test-user",
                RefreshToken = "refresh-token",
                RefreshTokenExpiresAt = DateTime.UtcNow.AddHours(1),
                UserDeviceId = "correct-device-id"
            };

            // Act
            bool result = token.IsValid("refresh-token", "wrong-device-id");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Revoke_SetsIsRevokedTrue()
        {
            // Arrange
            var token = new Token
            {
                Id = "test-id",
                UserId = "test-user",
                RefreshToken = "refresh-token",
                RefreshTokenExpiresAt = DateTime.UtcNow.AddHours(1),
                UserDeviceId = "device-id"
            };

            // Act
            token.Revoke();

            // Assert
            Assert.True(token.IsRevoked);
        }

        [Fact]
        public void BlackList_SetsIsBlackListedTrue()
        {
            // Arrange
            var token = new Token
            {
                Id = "test-id",
                UserId = "test-user",
                RefreshToken = "refresh-token",
                RefreshTokenExpiresAt = DateTime.UtcNow.AddHours(1),
                UserDeviceId = "device-id"
            };

            // Act
            token.BlackList();

            // Assert
            Assert.True(token.IsBlackListed);
        }

        [Fact]
        public void IsValid_WhenBothRevokedAndBlacklisted_ReturnsFalse()
        {
            // Arrange
            var token = new Token
            {
                Id = "test-id",
                UserId = "test-user",
                RefreshToken = "refresh-token",
                RefreshTokenExpiresAt = DateTime.UtcNow.AddHours(1),
                UserDeviceId = "device-id",
                IsRevoked = true,
                IsBlackListed = true
            };

            // Act
            bool result = token.IsValid("refresh-token", "device-id");

            // Assert
            Assert.False(result);
        }
    }
}
