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
        Task<DeviceVerificationResult> SendVerification(ApplicationUser user, Device device);

        /// <summary>
        /// Verify a device and mark it as trusted by providing the code sent to the user for this device
        /// </summary>
        /// <param name="device"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<DeviceVerificationResult> Verify(Device device, string code);
    }

    public class DeviceVerificationError : IResultError
    {
        public required string Code { get; set; }
        public required string? Message { get; set; }
    }

    /// <summary>
    /// Base result object for <see cref="IDeviceVerificationService"/>
    /// </summary>
    public class DeviceVerificationResult : IResult
    {
        public bool Succeeded { get; set; } = false;
        public ICollection<IResultError> Errors { get; set; } = [];

        public void AddError(string code, string? message = null)
        {
            Errors.Add(new DeviceVerificationError { Code = code, Message = message });
        }
    }
}
