using Identity.Core.Models;

namespace Identity.Core.Tests
{
    public class LoginAttemptTests
    {
        [Fact]
        public void IsValid_WhenStatusIsNotTwoFactorAuthenticationSent_ReturnsFalse()
        {
            // Arrange
            var loginAttempt = new LoginAttempt
            {
                UserId = "test-user",
                IpAddress = "127.0.0.1",
                UserAgent = "test-agent",
                Status = LoginStatus.FAILED
            };

            // Act
            bool result = loginAttempt.IsValid(5);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_WhenWithinTimeWindow_ReturnsTrue()
        {
            // Arrange
            var loginAttempt = new LoginAttempt
            {
                UserId = "test-user",
                IpAddress = "127.0.0.1",
                UserAgent = "test-agent",
                Status = LoginStatus.TwoFactorAuthenticationSent,
                CreatedAt = DateTime.UtcNow
            };

            // Act
            bool result = loginAttempt.IsValid(5);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_WhenOutsideTimeWindow_ReturnsFalse()
        {
            // Arrange
            var loginAttempt = new LoginAttempt
            {
                UserId = "test-user",
                IpAddress = "127.0.0.1",
                UserAgent = "test-agent",
                Status = LoginStatus.TwoFactorAuthenticationSent,
                CreatedAt = DateTime.UtcNow.AddMinutes(-10)
            };

            // Act
            bool result = loginAttempt.IsValid(5);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(LoginStatus.SUCCESS)]
        [InlineData(LoginStatus.FAILED)]
        [InlineData(LoginStatus.BLOCKED)]
        public void IsValid_WithNonTwoFactorStatus_ReturnsFalse(LoginStatus status)
        {
            // Arrange
            var loginAttempt = new LoginAttempt
            {
                UserId = "test-user",
                IpAddress = "127.0.0.1",
                UserAgent = "test-agent",
                Status = status,
                CreatedAt = DateTime.UtcNow
            };

            // Act
            bool result = loginAttempt.IsValid(5);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public void IsValid_WithDifferentTimeWindows_ValidatesCorrectly(double windowTime)
        {
            // Arrange
            var loginAttempt = new LoginAttempt
            {
                UserId = "test-user",
                IpAddress = "127.0.0.1",
                UserAgent = "test-agent",
                Status = LoginStatus.TwoFactorAuthenticationSent,
                CreatedAt = DateTime.UtcNow.AddMinutes(-(windowTime - 0.5))
            };

            // Act
            bool result = loginAttempt.IsValid(windowTime);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Constructor_InitializesDefaultProperties()
        {
            // Arrange & Act
            var loginAttempt = new LoginAttempt
            {
                UserId = "test-user",
                IpAddress = "127.0.0.1",
                UserAgent = "test-agent"
            };

            // Assert
            Assert.NotNull(loginAttempt.Id);
            Assert.Equal(LoginStatus.FAILED, loginAttempt.Status);
            Assert.Null(loginAttempt.FailureReason);
            Assert.NotEqual(default(DateTime), loginAttempt.CreatedAt);
            Assert.NotNull(loginAttempt.TwoFactorCodeAttempts);
            Assert.Empty(loginAttempt.TwoFactorCodeAttempts);
        }
    }
}
