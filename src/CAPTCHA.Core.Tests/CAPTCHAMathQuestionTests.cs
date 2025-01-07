using CAPTCHA.Core.Models;

namespace CAPTCHA.Core.Tests
{
    public class CAPTCHAMathQuestionTests
    {
        [Fact]
        public void IsValid_ShouldReturnFalse_WhenExpired()
        {
            // Arrange
            var captcha = new CAPTCHAMathQuestion
            {
                ExpiresAt = DateTime.UtcNow.AddSeconds(-1) // Already expired
            };

            // Act
            var result = captcha.IsValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenAlreadySuccessful()
        {
            // Arrange
            var captcha = new CAPTCHAMathQuestion
            {
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsSuccessful = true
            };

            // Act
            var result = captcha.IsValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenValid()
        {
            // Arrange
            var captcha = new CAPTCHAMathQuestion
            {
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsSuccessful = false
            };

            // Act
            var result = captcha.IsValid();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CheckAnswer_ShouldReturnTrue_ForCorrectAnswer()
        {
            // Arrange
            var captcha = new CAPTCHAMathQuestion
            {
                Answer = 42.0,
                ExpiresAt = DateTime.UtcNow.AddMinutes(1)
            };

            // Act
            var result = captcha.CheckAnswer(42.0);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CheckAnswer_ShouldReturnFalse_ForIncorrectAnswer()
        {
            // Arrange
            var captcha = new CAPTCHAMathQuestion
            {
                Answer = 42.0,
                ExpiresAt = DateTime.UtcNow.AddMinutes(1)
            };

            // Act
            var result = captcha.CheckAnswer(41.9999);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void MarkAsSuccessful_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var captcha = new CAPTCHAMathQuestion()
            {
                ExpiresAt = DateTime.UtcNow.AddMinutes(1)
            };

            // Act
            captcha.MarkAsSuccessful();

            // Assert
            Assert.True(captcha.IsSuccessful);
            Assert.NotNull(captcha.SuccessfulAt);
        }

        [Fact]
        public void SetUser_ShouldSetUserPropertiesCorrectly()
        {
            // Arrange
            var captcha = new CAPTCHAMathQuestion()
            {
                ExpiresAt = DateTime.UtcNow.AddMinutes(1)
            };
            var userAgent = "TestUserAgent";
            var ipAddress = "127.0.0.1";

            // Act
            captcha.SetUser(userAgent, ipAddress);

            // Assert
            Assert.Equal(userAgent, captcha.UserAgent);
            Assert.Equal(ipAddress, captcha.IPAddress);
        }

        [Fact]
        public void UserEntriesExists_ShouldReturnTrue_WhenUserSet()
        {
            // Arrange
            var captcha = new CAPTCHAMathQuestion() { ExpiresAt = DateTime.UtcNow.AddMinutes(1) };
            captcha.SetUser("TestUserAgent", "127.0.0.1");

            // Act
            var result = captcha.UserEntriesExists();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void UserEntriesExists_ShouldReturnFalse_WhenUserNotSet()
        {
            // Arrange
            var captcha = new CAPTCHAMathQuestion() { ExpiresAt = DateTime.UtcNow.AddMinutes(1) };

            // Act
            var result = captcha.UserEntriesExists();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Suspicious_ShouldReturnTrue_ForMismatchedUserAgentOrIpAddress()
        {
            // Arrange
            var captcha = new CAPTCHAMathQuestion() { ExpiresAt = DateTime.UtcNow.AddMinutes(1) };
            captcha.SetUser("TestUserAgent", "127.0.0.1");

            // Act
            var result = captcha.Suspicious("OtherUserAgent", "127.0.0.2");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Suspicious_ShouldReturnFalse_ForMatchingUserAgentAndIpAddress()
        {
            // Arrange
            var captcha = new CAPTCHAMathQuestion() { ExpiresAt = DateTime.UtcNow.AddMinutes(1) };
            captcha.SetUser("TestUserAgent", "127.0.0.1");

            // Act
            var result = captcha.Suspicious("TestUserAgent", "127.0.0.1");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IncrementAttempts_ShouldIncreaseAttemptsCount()
        {
            // Arrange
            var captcha = new CAPTCHAMathQuestion() { ExpiresAt = DateTime.UtcNow.AddMinutes(1) };

            // Act
            captcha.IncrementAttempts();

            // Assert
            Assert.Equal(1, captcha.Attempts);
        }

        [Fact]
        public void MaxAttemptLimitReached_ShouldReturnTrue_WhenLimitReached()
        {
            // Arrange
            var captcha = new CAPTCHAMathQuestion() { ExpiresAt = DateTime.UtcNow.AddMinutes(1) };
            captcha.Attempts = 3;

            // Act
            var result = captcha.MaxAttemptLimitReached();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void MaxAttemptLimitReached_ShouldReturnFalse_WhenBelowLimit()
        {
            // Arrange
            var captcha = new CAPTCHAMathQuestion() { ExpiresAt = DateTime.UtcNow.AddMinutes(1) };
            captcha.Attempts = 2;

            // Act
            var result = captcha.MaxAttemptLimitReached();

            // Assert
            Assert.False(result);
        }
    }
}