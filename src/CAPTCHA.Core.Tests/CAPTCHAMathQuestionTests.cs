using CAPTCHA.Core.Models;

namespace CAPTCHA.Core.Tests
{
    public class CAPTCHAMathQuestionTests
    {
        [Fact]
        public void IsValid_ReturnsTrue_WhenNotExpiredAndNotSuccessful()
        {
            // Arrange
            var question = new CAPTCHAMathQuestion
            {
                Answer = "42",
                ExpiresAt = DateTime.UtcNow.AddMinutes(5)
            };

            // Act
            var result = question.IsValid();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_ReturnsFalse_WhenExpired()
        {
            // Arrange
            var question = new CAPTCHAMathQuestion
            {
                Answer = "42",
                ExpiresAt = DateTime.UtcNow.AddMinutes(-1)
            };

            // Act
            var result = question.IsValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_ReturnsFalse_WhenAlreadySuccessful()
        {
            // Arrange
            var question = new CAPTCHAMathQuestion
            {
                Answer = "42",
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsSuccessful = true
            };

            // Act
            var result = question.IsValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void MarkAsSuccessful_SetsIsSuccessfulAndSuccessfulAt()
        {
            // Arrange
            var question = new CAPTCHAMathQuestion
            {
                Answer = "42",
                ExpiresAt = DateTime.UtcNow.AddMinutes(5)
            };

            // Act
            question.MarkAsSuccessful();

            // Assert
            Assert.True(question.IsSuccessful);
            Assert.NotNull(question.SuccessfulAt);
            Assert.True(question.SuccessfulAt <= DateTime.UtcNow);
        }

        [Fact]
        public void SetUser_SetsUserAgentAndIPAddress()
        {
            // Arrange
            var question = new CAPTCHAMathQuestion
            {
                Answer = "42",
                ExpiresAt = DateTime.UtcNow.AddMinutes(5)
            };
            var userAgent = "Mozilla/5.0";
            var ipAddress = "127.0.0.1";

            // Act
            question.SetUser(userAgent, ipAddress);

            // Assert
            Assert.Equal(userAgent, question.UserAgent);
            Assert.Equal(ipAddress, question.IPAddress);
        }

        [Fact]
        public void IncrementAttempts_IncreasesAttemptsByOne()
        {
            // Arrange
            var question = new CAPTCHAMathQuestion
            {
                Answer = "42",
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                Attempts = 2
            };

            // Act
            question.IncrementAttempts();

            // Assert
            Assert.Equal(3, question.Attempts);
        }

        [Fact]
        public void DefaultValues_AreSetCorrectly()
        {
            // Arrange
            var question = new CAPTCHAMathQuestion
            {
                Answer = "42",
                ExpiresAt = DateTime.UtcNow.AddMinutes(5)
            };

            // Act & Assert
            Assert.False(question.IsSuccessful);
            Assert.Equal(0, question.Attempts);
            Assert.NotNull(question.Id);
            Assert.Null(question.SuccessfulAt);
            Assert.Null(question.UserAgent);
            Assert.Null(question.IPAddress);
        }
    }
}
