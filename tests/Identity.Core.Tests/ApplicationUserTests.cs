using Identity.Core.Models;

namespace Identity.Core.Tests
{
    [TestClass]
    public class ApplicationUserTests
    {
        [TestMethod]
        public void MarkEmailConfirmed_ShouldSetEmailConfirmedToTrue()
        {
            var user = new ApplicationUser
            {
                UserName = "user1",
                Email = "user1@example.com",
                PhoneNumber = "1234567890",
                PasswordHash = "hash"
            };

            user.MarkEmailConfirmed();

            Assert.IsTrue(user.EmailConfirmed);
        }

        [TestMethod]
        public void MarkPhoneNumberConfirmed_ShouldSetPhoneNumberConfirmedToTrue()
        {
            var user = new ApplicationUser
            {
                UserName = "user2",
                Email = "user2@example.com",
                PhoneNumber = "1234567890",
                PasswordHash = "hash"
            };

            user.MarkPhoneNumberConfirmed();

            Assert.IsTrue(user.PhoneNumberConfirmed);
        }

        [TestMethod]
        public void ResetTotp_ShouldClearTotpSecretAndSetTotpConfirmedFalse()
        {
            var user = new ApplicationUser
            {
                UserName = "user3",
                Email = "user3@example.com",
                PhoneNumber = "1234567890",
                PasswordHash = "hash",
                TotpSecret = "secret",
                TotpConfirmed = true
            };

            user.ResetTotp();

            Assert.IsNull(user.TotpSecret);
            Assert.IsFalse(user.TotpConfirmed);
        }

        [TestMethod]
        public void MarkTotpConfirmed_ShouldSetTotpConfirmedToTrue()
        {
            var user = new ApplicationUser
            {
                UserName = "user4",
                Email = "user4@example.com",
                PhoneNumber = "1234567890",
                PasswordHash = "hash",
                TotpConfirmed = false
            };

            user.MarkTotpConfirmed();

            Assert.IsTrue(user.TotpConfirmed);
        }

        [TestMethod]
        public void ToString_ShouldContainUserInfo()
        {
            var user = new ApplicationUser
            {
                UserName = "user5",
                Email = "user5@example.com",
                PhoneNumber = "1234567890",
                PasswordHash = "hash"
            };

            var output = user.ToString();

            StringAssert.Contains(output, "Application User");
            StringAssert.Contains(output, user.UserName);
            StringAssert.Contains(output, user.Email);
            StringAssert.Contains(output, user.PhoneNumber);
        }
    }

}
