using Identity.Core.Models;

namespace Identity.Core.Tests
{
    [TestClass]
    public class TokenTests
    {
        private Token _token;
        private string _deviceId = "device123";

        [TestInitialize]
        public void Setup()
        {
            _token = new Token
            {
                Id = "token123",
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                DeviceId = _deviceId,
                UserId = "user123"
            };
        }

        [TestMethod]
        public void IsValid_ShouldReturnTrue_WhenNotRevokedUsedExpiredAndDeviceMatches()
        {
            Assert.IsTrue(_token.IsValid(_deviceId));
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenRevoked()
        {
            _token.Revoke();
            Assert.IsFalse(_token.IsValid(_deviceId));
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenUsed()
        {
            _token.MarkUsed();
            Assert.IsFalse(_token.IsValid(_deviceId));
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenExpired()
        {
            _token.ExpiresAt = DateTime.UtcNow.AddSeconds(-1);
            Assert.IsFalse(_token.IsValid(_deviceId));
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenDeviceDoesNotMatch()
        {
            Assert.IsFalse(_token.IsValid("wrongDeviceId"));
        }

        [TestMethod]
        public void Revoke_ShouldSetRevokedAtToCurrentTime()
        {
            Assert.IsNull(_token.RevokedAt);

            _token.Revoke();

            Assert.IsNotNull(_token.RevokedAt);
            Assert.IsTrue((_token.RevokedAt.Value - DateTime.UtcNow).TotalSeconds < 5);
        }

        [TestMethod]
        public void MarkUsed_ShouldSetUsedAtToCurrentTime()
        {
            Assert.IsNull(_token.UsedAt);

            _token.MarkUsed();

            Assert.IsNotNull(_token.UsedAt);
            Assert.IsTrue((_token.UsedAt.Value - DateTime.UtcNow).TotalSeconds < 5);
        }
    }
}
