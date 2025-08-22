using Identity.Core.Models;

namespace Identity.Core.Tests
{
    [TestClass]
    public class DeviceTests
    {
        [TestMethod]
        public void MarkTrusted_ShouldSetIsTrustedToTrue()
        {
            var device = new Device
            {
                Id = "device123",
                Name = "Test Device",
                UserId = "user123"
            };

            Assert.IsFalse(device.IsTrusted);

            device.MarkTrusted();

            Assert.IsTrue(device.IsTrusted);
        }
    }
}
