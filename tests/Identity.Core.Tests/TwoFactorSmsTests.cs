using Identity.Core.Models;

namespace Identity.Core.Tests
{
    [TestClass]
    public class TwoFactorSmsTests
    {
        private TwoFactorSms _twoFactorSms;

        [TestInitialize]
        public void Setup()
        {
            _twoFactorSms = new TwoFactorSms
            {
                Code = "123456",
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                PhoneNumber = "1234567890",
                UserId = "user123",
                LoginId = "login123"
            };
        }

        [TestMethod]
        public void IsValid_ShouldReturnTrue_WhenNotUsedAndNotExpired()
        {
            Assert.IsTrue(_twoFactorSms.IsValid());
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenUsed()
        {
            _twoFactorSms.MarkUsed();
            Assert.IsFalse(_twoFactorSms.IsValid());
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenExpired()
        {
            _twoFactorSms.ExpiresAt = DateTime.UtcNow.AddSeconds(-1);
            Assert.IsFalse(_twoFactorSms.IsValid());
        }

        [TestMethod]
        public void MarkUsed_ShouldSetUsedAtToCurrentTime()
        {
            Assert.IsNull(_twoFactorSms.UsedAt);

            _twoFactorSms.MarkUsed();

            Assert.IsNotNull(_twoFactorSms.UsedAt);
            Assert.IsTrue((_twoFactorSms.UsedAt.Value - DateTime.UtcNow).TotalSeconds < 5);
        }
    }
}
