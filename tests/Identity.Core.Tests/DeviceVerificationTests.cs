using Identity.Core.Models;

namespace Identity.Core.Tests
{
    [TestClass]
    public class DeviceVerificationTests
    {
        private DeviceVerification _verification;

        [TestInitialize]
        public void Setup()
        {
            _verification = new DeviceVerification
            {
                Email = "user@example.com",
                Code = "123456",
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                UserId = "user123",
                DeviceId = "device123"
            };
        }

        [TestMethod]
        public void IsValid_ShouldReturnTrue_WhenNotUsedAndNotExpired()
        {
            Assert.IsTrue(_verification.IsValid());
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenUsed()
        {
            _verification.MarkUsed();
            Assert.IsFalse(_verification.IsValid());
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenExpired()
        {
            _verification.ExpiresAt = DateTime.UtcNow.AddSeconds(-1);
            Assert.IsFalse(_verification.IsValid());
        }

        [TestMethod]
        public void MarkUsed_ShouldSetUsedAtToCurrentTime()
        {
            Assert.IsNull(_verification.UsedAt);

            _verification.MarkUsed();

            Assert.IsNotNull(_verification.UsedAt);
            Assert.IsTrue((_verification.UsedAt.Value - DateTime.UtcNow).TotalSeconds < 5);
        }
    }
}
