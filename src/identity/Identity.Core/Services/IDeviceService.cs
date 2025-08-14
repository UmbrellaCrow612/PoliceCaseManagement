using Identity.Core.Models;
using Identity.Core.ValueObjects;

namespace Identity.Core.Services
{
    /// <summary>
    /// Business contract to interact with user devices
    /// </summary>
    public interface IDeviceService
    {
        /// <summary>
        /// Get a user's device 
        /// </summary>
        /// <param name="userId">The user who's device you are getting</param>
        /// <param name="info">Information about the device</param>
        Task<Device?> GetDeviceAsync(string userId, DeviceInfo info);
    }
}
