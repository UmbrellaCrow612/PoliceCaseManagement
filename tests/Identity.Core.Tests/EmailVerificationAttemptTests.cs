using Identity.Core.Models;

namespace Identity.Core.Tests
{
    public class EmailVerificationAttemptTests
    {
        [Fact]
        public void IsValid_ReturnsFalse_WhenIsSuccessfulIsTrue()
        {
            // Arrange
            var attempt = new EmailVerificationAttempt
            {
                IsSuccessful = true,
                Code = "test",
                Email = "test@email.com",
                UserId = "test_user"
            };

            // Act
            var result = attempt.IsValid(5);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_ReturnsFalse_WhenWindowTimeHasExpired()
        {
            // Arrange
            var attempt = new EmailVerificationAttempt
            {
                CreatedAt = DateTime.UtcNow.AddMinutes(-10),
                Code = "test",
                Email = "test@email.com",
                UserId = "test_user"
            };

            // Act
            var result = attempt.IsValid(5);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_ReturnsTrue_WhenWithinWindowTimeAndNotSuccessful()
        {
            // Arrange
            var attempt = new EmailVerificationAttempt
            {
                CreatedAt = DateTime.UtcNow.AddMinutes(-3),
                Code = "test",
                Email = "test@email.com",
                UserId = "test_user"
            };

            // Act
            var result = attempt.IsValid(5);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void MarkUsed_SetsIsSuccessfulToTrue_AndSetsUsedAt()
        {
            // Arrange
            var attempt = new EmailVerificationAttempt
            {
                Code = "test",
                Email = "test@email.com",
                UserId = "test_user"
            };

            // Act
            attempt.MarkUsed();

            // Assert
            Assert.True(attempt.IsSuccessful);
            Assert.NotNull(attempt.UsedAt);
            Assert.True(attempt.UsedAt <= DateTime.UtcNow);
        }
    }
}
