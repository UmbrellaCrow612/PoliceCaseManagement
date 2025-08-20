using Identity.Core.Models;
using Identity.Core.ValueObjects;
using Results.Abstractions;

namespace Identity.Core.Services
{
    /// <summary>
    /// Business contract to interact with user devices
    /// </summary>
    public interface IDeviceService
    {
        /// <summary>
        /// Get a user's device by there ID and the information about the device
        /// </summary>
        /// <param name="userId">The user who's device you are getting</param>
        /// <param name="info">Information about the device</param>
        Task<Device?> GetDeviceAsync(string userId, DeviceInfo info);

        /// <summary>
        /// Check if a device exists
        /// </summary>
        /// <param name="userId">The user ID of the device to check</param>
        /// <param name="info">The info of the device</param>
        Task<bool> ExistsAsync(string userId, DeviceInfo info);

        /// <summary>
        /// Find a device by it's ID
        /// </summary>
        /// <param name="deviceId">The ID of the device</param>
        Task<Device?> FindByIdAsync(string deviceId);

        /// <summary>
        /// Create a user device into the system
        /// </summary>
        /// <param name="user">The user to create it for </param>
        /// <param name="deviceInfo">The info about the device </param>
        Task<IResult> CreateAsync(ApplicationUser user, DeviceInfo deviceInfo);
    }
}
