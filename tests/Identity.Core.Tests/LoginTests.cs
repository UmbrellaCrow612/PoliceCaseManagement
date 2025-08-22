using Identity.Core.Models;

namespace Identity.Core.Tests
{
    [TestClass]
    public class LoginTests
    {
        private Login _login;

        [TestInitialize]
        public void Setup()
        {
            _login = new Login
            {
                Status = LoginStatus.TwoFactorAuthenticationReached,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                UserId = "user123",
                DeviceId = "device123"
            };
        }

        [TestMethod]
        public void IsValid_ShouldReturnTrue_WhenStatusIsTwoFactorAndNotUsedOrExpired()
        {
            Assert.IsTrue(_login.IsValid());
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenStatusIsNotTwoFactor()
        {
            _login.Status = LoginStatus.FAILED;
            Assert.IsFalse(_login.IsValid());

            _login.Status = LoginStatus.SUCCESS;
            Assert.IsFalse(_login.IsValid());
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenUsed()
        {
            _login.MarkUsed();
            Assert.IsFalse(_login.IsValid());
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenExpired()
        {
            _login.ExpiresAt = DateTime.UtcNow.AddSeconds(-1);
            Assert.IsFalse(_login.IsValid());
        }

        [TestMethod]
        public void MarkUsed_ShouldSetStatusToSuccessAndUsedAtToCurrentTime()
        {
            Assert.AreEqual(LoginStatus.TwoFactorAuthenticationReached, _login.Status);
            Assert.IsNull(_login.UsedAt);

            _login.MarkUsed();

            Assert.AreEqual(LoginStatus.SUCCESS, _login.Status);
            Assert.IsNotNull(_login.UsedAt);
            Assert.IsTrue((_login.UsedAt.Value - DateTime.UtcNow).TotalSeconds < 5);
        }
    }
}
