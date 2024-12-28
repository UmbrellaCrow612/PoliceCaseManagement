using Identity.Core.Models;

namespace Identity.Core.Tests
{
    public class ApplicationUserTests
    {
        [Fact]
        public void IsLinkedToADepartment_ReturnsFalse_WhenDepartmentIdIsNull()
        {
            // Arrange
            var user = new ApplicationUser();

            // Act
            var result = user.IsLinkedToADepartment();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsLinkedToADepartment_ReturnsTrue_WhenDepartmentIdIsNotNull()
        {
            // Arrange
            var user = new ApplicationUser { DepartmentId = "123" };

            // Act
            var result = user.IsLinkedToADepartment();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void LinkToDepartment_SetsDepartmentIdCorrectly()
        {
            // Arrange
            var user = new ApplicationUser();

            // Act
            user.LinkToDepartment("123");

            // Assert
            Assert.Equal("123", user.DepartmentId);
        }

        [Fact]
        public void LastUsedDevice_ReturnsNull_WhenLastLoginDeviceIdIsNull()
        {
            // Arrange
            var user = new ApplicationUser();

            // Act
            var result = user.LastUsedDevice();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void LastUsedDevice_ReturnsLastLoginDeviceId_WhenNotNull()
        {
            // Arrange
            var user = new ApplicationUser { LastLoginDeviceId = "Device123" };

            // Act
            var result = user.LastUsedDevice();

            // Assert
            Assert.Equal("Device123", result);
        }
    }
}
