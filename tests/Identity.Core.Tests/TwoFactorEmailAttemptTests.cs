using Identity.Core.Models;

namespace Identity.Core.Tests
{
    public class TwoFactorEmailAttemptTests
    {
        [Fact]
        public void Constructor_SetsDefaultValues()
        {
            // Arrange & Act
            var attempt = new TwoFactorEmailAttempt
            {
                Email = "test@example.com",
                Code = "123456",
                LoginAttemptId = "test-login-id"
            };

            // Assert
            Assert.NotNull(attempt.Id);
            Assert.False(string.IsNullOrEmpty(attempt.Id));
            Assert.Equal("test@example.com", attempt.Email);
            Assert.Equal("123456", attempt.Code);
            Assert.Equal("test-login-id", attempt.LoginAttemptId);
            Assert.False(attempt.IsSuccessful);
            Assert.Null(attempt.SuccessfulAt);
            Assert.Null(attempt.LoginAttempt);
            Assert.True((DateTime.UtcNow - attempt.CreatedAt).TotalSeconds < 5);
        }

        [Theory]
        [InlineData(5)]  
        [InlineData(10)] 
        [InlineData(15)] 
        public void IsValid_WithinTimeWindow_ReturnsTrue(double windowTimeMinutes)
        {
            // Arrange
            var attempt = new TwoFactorEmailAttempt
            {
                Email = "test@example.com",
                Code = "123456",
                LoginAttemptId = "test-login-id"
            };

            // Act
            bool isValid = attempt.IsValid(windowTimeMinutes);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void IsValid_WhenSuccessful_ReturnsFalse()
        {
            // Arrange
            var attempt = new TwoFactorEmailAttempt
            {
                Email = "test@example.com",
                Code = "123456",
                LoginAttemptId = "test-login-id",
                IsSuccessful = true,
                SuccessfulAt = DateTime.UtcNow
            };

            // Act
            bool isValid = attempt.IsValid(10);

            // Assert
            Assert.False(isValid);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(15)]
        public void IsValid_AfterTimeWindow_ReturnsFalse(double windowTimeMinutes)
        {
            // Arrange
            var attempt = new TwoFactorEmailAttempt
            {
                Email = "test@example.com",
                Code = "123456",
                LoginAttemptId = "test-login-id",
                CreatedAt = DateTime.UtcNow.AddMinutes(-windowTimeMinutes - 1)
            };

            // Act
            bool isValid = attempt.IsValid(windowTimeMinutes);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void MarkUsed_SetsIsSuccessfulToTrue()
        {
            // Arrange
            var attempt = new TwoFactorEmailAttempt
            {
                Email = "test@example.com",
                Code = "123456",
                LoginAttemptId = "test-login-id"
            };

            // Act
            attempt.MarkUsed();

            // Assert
            Assert.True(attempt.IsSuccessful);
        }

        [Fact]
        public void MarkUsed_SetsSuccessfulAtToCurrentTime()
        {
            // Arrange
            var attempt = new TwoFactorEmailAttempt
            {
                Email = "test@example.com",
                Code = "123456",
                LoginAttemptId = "test-login-id"
            };

            // Act
            attempt.MarkUsed();

            // Assert
            Assert.NotNull(attempt.SuccessfulAt);
            Assert.True((DateTime.UtcNow - attempt.SuccessfulAt.Value).TotalSeconds < 5); // Marked within last 5 seconds
        }
    }
}
