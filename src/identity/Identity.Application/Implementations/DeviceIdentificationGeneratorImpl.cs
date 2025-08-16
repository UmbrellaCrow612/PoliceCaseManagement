using Identity.Core.Services;
using Identity.Core.ValueObjects;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Application.Implementations
{
    /// <summary>
    /// Business implementation of the contract <see cref="IDeviceIdentificationGenerator"/> - test this, as well when using it else where only use the <see cref="IDeviceIdentificationGenerator"/>
    /// interface not this class
    /// </summary>
    public class DeviceIdentificationGeneratorImpl : IDeviceIdentificationGenerator
    {
        public string GenerateId(string userId, DeviceInfo deviceInfo)
        {
            // Combine the parts into one string (separator prevents accidental collisions)
            string combined = $"{userId}|{deviceInfo.DeviceFingerPrint}|{deviceInfo.IpAddress}|{deviceInfo.UserAgent}";

            // Convert to bytes
            byte[] inputBytes = Encoding.UTF8.GetBytes(combined);

            // Compute the hash
            byte[] hashBytes = SHA256.HashData(inputBytes);

            // Convert hash to a Base64 string for a shorter ID
            return Convert.ToBase64String(hashBytes)
                .Replace("/", "_")  // URL-safe
                .Replace("+", "-")
                .TrimEnd('=');
        }
    }
}
