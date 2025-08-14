using Identity.Core.Models;
using Identity.Core.Services;
using Identity.Core.ValueObjects;

namespace Identity.Application.Implementations
{
    /// <summary>
    /// Business implementation of the contract <see cref="IDeviceService"/> - test this, as well when using it else where only use the <see cref="IDeviceService"/>
    /// interface not this class
    /// </summary>
    public class DeviceServiceImpl : IDeviceService
    {
        public Task<Device?> GetDeviceAsync(string userId, DeviceInfo info)
        {
            throw new NotImplementedException();
        }
    }
}
