using System.Security.Cryptography;
using System.Text;
using UAParser;

namespace Identity.API.Helpers
{
    public class DeviceIdentification(ILogger<DeviceIdentification> logger) : IDeviceIdentification
    {
        private readonly ILogger<DeviceIdentification> _logger = logger;

        public string GenerateDeviceId(string userId, string userAgent, string deviceFingerPrint)
        {
            var parser = Parser.GetDefault();

            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            if (string.IsNullOrWhiteSpace(userAgent))
                throw new ArgumentException("User agent cannot be null or empty.", nameof(userAgent));

            if (string.IsNullOrWhiteSpace(deviceFingerPrint))
                throw new ArgumentException("Finger print cannot be null or empty.", nameof(deviceFingerPrint));

            try
            {
                // Parse the user agent
                var clientInfo = parser.Parse(userAgent);

                // Create components for device ID
                var components = new[]
                {
                    clientInfo.UA.Family,
                    clientInfo.UA.Major,
                    clientInfo.OS.Family,
                    clientInfo.OS.Major,
                    clientInfo.Device.Family,
                    userId,
                    deviceFingerPrint
                };

                // Join components and create a hash
                var deviceIdSource = string.Join("-", components);
                var deviceId = ComputeDeviceIdHash(deviceIdSource);

                _logger.LogInformation(
                    "Generated Device ID for UA: {UserAgent}, ID: {userId}, Device: {DeviceFamily}",
                    clientInfo.UA.Family,
                    userId,
                    clientInfo.Device.Family);

                return deviceId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating device ID for UA: {UserAgent}", userAgent);
                throw;
            }
        }

        private static string ComputeDeviceIdHash(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = SHA256.HashData(inputBytes);
            return Convert.ToBase64String(hashBytes)
                .Replace("+", "")  // Remove special characters
                .Replace("/", "")
                .Replace("=", "")
                    [..32];  // Truncate to a fixed length
        }
    }
}
