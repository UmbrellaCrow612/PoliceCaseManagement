using Identity.Core.Models;

namespace Identity.Core.Tests
{
    public class PasswordResetAttemptTests
    {
        [Fact]
        public void Constructor_InitializesDefaultProperties()
        {
            // Arrange & Act
            var attempt = new PasswordResetAttempt
            {
                UserId = "test-user",
                Code = "123456"
            };

            // Assert
            Assert.NotNull(attempt.Id);
            Assert.Null(attempt.User);
            Assert.NotEqual(default(DateTime), attempt.CreatedAt);
            Assert.Null(attempt.SuccessfulAt);
            Assert.False(attempt.IsSuccessful);
            Assert.False(attempt.IsRevoked);
        }

        [Fact]
        public void IsValid_WhenNew_ReturnsTrue()
        {
            // Arrange
            var attempt = new PasswordResetAttempt
            {
                UserId = "test-user",
                Code = "123456",
                CreatedAt = DateTime.UtcNow
            };

            // Act
            bool result = attempt.IsValid(30);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_WhenRevoked_ReturnsFalse()
        {
            // Arrange
            var attempt = new PasswordResetAttempt
            {
                UserId = "test-user",
                Code = "123456",
                CreatedAt = DateTime.UtcNow,
                IsRevoked = true
            };

            // Act
            bool result = attempt.IsValid(30);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_WhenSuccessful_ReturnsFalse()
        {
            // Arrange
            var attempt = new PasswordResetAttempt
            {
                UserId = "test-user",
                Code = "123456",
                CreatedAt = DateTime.UtcNow,
                IsSuccessful = true
            };

            // Act
            bool result = attempt.IsValid(30);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(30)]
        public void IsValid_WithinTimeWindow_ReturnsTrue(double windowTime)
        {
            // Arrange
            var attempt = new PasswordResetAttempt
            {
                UserId = "test-user",
                Code = "123456",
                CreatedAt = DateTime.UtcNow.AddMinutes(-(windowTime - 1))
            };

            // Act
            bool result = attempt.IsValid(windowTime);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(30)]
        public void IsValid_OutsideTimeWindow_ReturnsFalse(double windowTime)
        {
            // Arrange
            var attempt = new PasswordResetAttempt
            {
                UserId = "test-user",
                Code = "123456",
                CreatedAt = DateTime.UtcNow.AddMinutes(-(windowTime + 1))
            };

            // Act
            bool result = attempt.IsValid(windowTime);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void MarkUsed_SetsSuccessfulProperties()
        {
            // Arrange
            var attempt = new PasswordResetAttempt
            {
                UserId = "test-user",
                Code = "123456"
            };

            // Act
            attempt.MarkUsed();

            // Assert
            Assert.True(attempt.IsSuccessful);
            Assert.NotNull(attempt.SuccessfulAt);
            Assert.InRange(
                attempt.SuccessfulAt!.Value,
                DateTime.UtcNow.AddSeconds(-1),
                DateTime.UtcNow.AddSeconds(1)
            );
        }

        [Fact]
        public void Revoke_SetsIsRevokedTrue()
        {
            // Arrange
            var attempt = new PasswordResetAttempt
            {
                UserId = "test-user",
                Code = "123456"
            };

            // Act
            attempt.Revoke();

            // Assert
            Assert.True(attempt.IsRevoked);
        }

        [Fact]
        public void IsValid_WhenBothRevokedAndSuccessful_ReturnsFalse()
        {
            // Arrange
            var attempt = new PasswordResetAttempt
            {
                UserId = "test-user",
                Code = "123456",
                CreatedAt = DateTime.UtcNow,
                IsRevoked = true,
                IsSuccessful = true
            };

            // Act
            bool result = attempt.IsValid(30);

            // Assert
            Assert.False(result);
        }
    }
}
