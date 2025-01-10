using Identity.Core.Models;

namespace Identity.Core.Tests
{
    public class TwoFactorSmsAttemptTests
    {
        [Fact]
        public void Constructor_SetsDefaultValues()
        {
            // Arrange
            var attempt = new TwoFactorSmsAttempt
            {
                LoginAttemptId = "test-login-id",
                Code = "123456",
                PhoneNumber = "+1234567890",
                UserId = "test-user-id"
            };

            // Assert
            Assert.NotNull(attempt.Id);
            Assert.False(attempt.IsSuccessful);
            Assert.Null(attempt.SuccessfulAt);
            Assert.Null(attempt.LoginAttempt);
            Assert.Null(attempt.User);
            Assert.InRange(attempt.CreatedAt, DateTime.UtcNow.AddSeconds(-5), DateTime.UtcNow.AddSeconds(5));
        }

        [Theory]
        [InlineData(5, 3, true)]  // Within window
        [InlineData(5, 6, false)] // Outside window
        [InlineData(10, 7, true)] // Within larger window
        public void IsValid_ChecksTimeWindow(double windowMinutes, double elapsedMinutes, bool expectedResult)
        {
            // Arrange
            var attempt = new TwoFactorSmsAttempt
            {
                LoginAttemptId = "test-login-id",
                Code = "123456",
                PhoneNumber = "+1234567890",
                UserId = "test-user-id",
                CreatedAt = DateTime.UtcNow.AddMinutes(-elapsedMinutes)
            };

            // Act
            var result = attempt.IsValid(windowMinutes);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void IsValid_ReturnsFalse_WhenAlreadySuccessful()
        {
            // Arrange
            var attempt = new TwoFactorSmsAttempt
            {
                LoginAttemptId = "test-login-id",
                Code = "123456",
                PhoneNumber = "+1234567890",
                UserId = "test-user-id",
                IsSuccessful = true
            };

            // Act
            var result = attempt.IsValid(5);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void MarkUsed_SetsSuccessfulProperties()
        {
            // Arrange
            var attempt = new TwoFactorSmsAttempt
            {
                LoginAttemptId = "test-login-id",
                Code = "123456",
                PhoneNumber = "+1234567890",
                UserId = "test-user-id"
            };

            // Act
            attempt.MarkUsed();

            // Assert
            Assert.True(attempt.IsSuccessful);
            Assert.NotNull(attempt.SuccessfulAt);
            Assert.InRange(attempt.SuccessfulAt!.Value, DateTime.UtcNow.AddSeconds(-5), DateTime.UtcNow.AddSeconds(5));
        }
    }
}
