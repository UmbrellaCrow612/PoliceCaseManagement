using Identity.Core.Models;

namespace Identity.Core.Tests
{
    public class PhoneConfirmationAttemptTests
    {
        [Fact]
        public void Constructor_InitializesDefaultProperties()
        {
            // Arrange & Act
            var attempt = new PhoneConfirmationAttempt
            {
                UserId = "test-user",
                Code = "123456",
                PhoneNumber = "+1234567890"
            };

            // Assert
            Assert.NotNull(attempt.Id);
            Assert.NotEqual(default(DateTime), attempt.CreatedAt);
            Assert.Null(attempt.User);
            Assert.False(attempt.IsUsed);
            Assert.Null(attempt.UsedAt);
        }

        [Fact]
        public void IsValid_WhenNew_ReturnsTrue()
        {
            // Arrange
            var attempt = new PhoneConfirmationAttempt
            {
                UserId = "test-user",
                Code = "123456",
                PhoneNumber = "+1234567890",
                CreatedAt = DateTime.UtcNow
            };

            // Act
            bool result = attempt.IsValid(30);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_WhenSuccessful_ReturnsFalse()
        {
            // Arrange
            var attempt = new PhoneConfirmationAttempt
            {
                UserId = "test-user",
                Code = "123456",
                PhoneNumber = "+1234567890",
                CreatedAt = DateTime.UtcNow,
                IsUsed = true
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
            var attempt = new PhoneConfirmationAttempt
            {
                UserId = "test-user",
                Code = "123456",
                PhoneNumber = "+1234567890",
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
            var attempt = new PhoneConfirmationAttempt
            {
                UserId = "test-user",
                Code = "123456",
                PhoneNumber = "+1234567890",
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
            var attempt = new PhoneConfirmationAttempt
            {
                UserId = "test-user",
                Code = "123456",
                PhoneNumber = "+1234567890"
            };

            // Act
            attempt.MarkUsed();

            // Assert
            Assert.True(attempt.IsUsed);
            Assert.NotNull(attempt.UsedAt);
            Assert.InRange(
                attempt.UsedAt!.Value,
                DateTime.UtcNow.AddSeconds(-1),
                DateTime.UtcNow.AddSeconds(1)
            );
        }

        [Fact]
        public void IsValid_WhenSuccessfulAndOutsideTimeWindow_ReturnsFalse()
        {
            // Arrange
            var attempt = new PhoneConfirmationAttempt
            {
                UserId = "test-user",
                Code = "123456",
                PhoneNumber = "+1234567890",
                CreatedAt = DateTime.UtcNow.AddMinutes(-31),
                IsUsed = true
            };

            // Act
            bool result = attempt.IsValid(30);

            // Assert
            Assert.False(result);
        }
    }
}
