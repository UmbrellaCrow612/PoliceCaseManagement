using Identity.Core.Models;

namespace Identity.Core.Tests
{
    [TestClass]
    public class PhoneVerificationTests
    {
        private PhoneVerification _phoneVerification;

        [TestInitialize]
        public void Setup()
        {
            _phoneVerification = new PhoneVerification
            {
                Code = "123456",
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                PhoneNumber = "1234567890",
                UserId = "user123"
            };
        }

        [TestMethod]
        public void IsValid_ShouldReturnTrue_WhenNotUsedAndNotExpired()
        {
            Assert.IsTrue(_phoneVerification.IsValid());
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenUsed()
        {
            _phoneVerification.MarkUsed();
            Assert.IsFalse(_phoneVerification.IsValid());
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenExpired()
        {
            _phoneVerification.ExpiresAt = DateTime.UtcNow.AddSeconds(-1);
            Assert.IsFalse(_phoneVerification.IsValid());
        }

        [TestMethod]
        public void MarkUsed_ShouldSetUsedAtToCurrentTime()
        {
            Assert.IsNull(_phoneVerification.UsedAt);

            _phoneVerification.MarkUsed();

            Assert.IsNotNull(_phoneVerification.UsedAt);
            Assert.IsTrue((_phoneVerification.UsedAt.Value - DateTime.UtcNow).TotalSeconds < 5);
        }
    }
}
