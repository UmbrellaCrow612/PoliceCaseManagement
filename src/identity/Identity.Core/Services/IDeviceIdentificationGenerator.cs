using Identity.Core.ValueObjects;

namespace Identity.Core.Services
{
    /// <summary>
    /// Defines a contract for generating device IDs.
    /// </summary>
    public interface IDeviceIdentificationGenerator
    {
        /// <summary>
        /// Generates a unique device ID based on a user's ID and device information.
        /// Combining the unique user ID with device-specific information ensures that 
        /// the resulting device ID is unique for each device associated with the user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="deviceInfo">Information about the device.</param>
        /// <returns>A unique device ID as a string.</returns>
        string GenerateId(string userId, DeviceInfo deviceInfo);
    }
}
