using Identity.Core.Models;
using Results.Abstractions;

namespace Identity.Core.Services
{
    /// <summary>
    /// Business contract contains all verification logic for a device
    /// </summary>
    public interface IDeviceVerificationService
    {
        /// <summary>
        /// Send a email device verification for a given device 
        /// </summary>
        /// <param name="device">The device to send it for</param>
        /// <param name="user">The user to send it to</param>
        Task<IResult> SendVerification(ApplicationUser user, Device device);

        /// <summary>
        /// Verify a device and mark it as trusted by providing the code sent to the user for this device
        /// </summary>
        /// <param name="device"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<IResult> Verify(Device device, string code);
    }
}
