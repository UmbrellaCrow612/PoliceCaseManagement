using Identity.API.Settings;
using Identity.Core.Models;
using Identity.Infrastructure.Data.Stores.Interfaces;
using UAParser;
using Utils.DTOs;

namespace Identity.API.Helpers
{
    public class DeviceManager(IUserDeviceStore userDeviceStore, IDeviceIdentification deviceIdentification)
    {
        private readonly IUserDeviceStore _userDeviceStore = userDeviceStore;
        private readonly IDeviceIdentification _deviceIdentification = deviceIdentification;

        /// <summary>
        /// Validates whether the provided user agent string contains the required properties for generating a device identifier.
        /// </summary>
        /// <param name="userAgent">The user agent string containing device and browser information. Must not be null or empty.</param>
        /// <returns>
        /// True if the user agent contains valid and non-empty properties required for device identification, 
        /// otherwise false.
        /// </returns>
        /// <remarks>
        /// This method checks for the presence of the following components in the user agent string:
        /// - User Agent family
        /// - Operating System family
        /// - Device family
        /// If any of these components are missing or invalid, the method returns false.
        /// </remarks>
        public bool UserAgentValid(string userAgent)
        {
            if (string.IsNullOrWhiteSpace(userAgent))
                return false;

            try
            {
                var parser = Parser.GetDefault();
                var clientInfo = parser.Parse(userAgent);

                // Check if required properties exist and are not null/empty
                return !string.IsNullOrWhiteSpace(clientInfo.UA?.Family) &&
                       !string.IsNullOrWhiteSpace(clientInfo.OS?.Family) &&
                       !string.IsNullOrWhiteSpace(clientInfo.Device?.Family);
            }
            catch
            {
                // Return false if any exception occurs during parsing
                return false;
            }
        }

        public (bool isValid, List<ErrorDetail> errors) VerifyRequestHasRequiredProperties(HttpRequest request)
        {
            List<ErrorDetail> errors = [];

            string userAgent = request.Headers.UserAgent.ToString();
            var deviceFingerprint = request.Headers[CustomHeaderOptions.XDeviceFingerprint].FirstOrDefault();

            if (!UserAgentValid(userAgent))
            {
                errors.Add(new ErrorDetail
                {
                    Field = "Request Headers",
                    Reason = "Request is missing device user agent or is malformed."
                });
            }

            if (string.IsNullOrWhiteSpace(deviceFingerprint))
            {
                errors.Add(new ErrorDetail
                {
                    Field = "Request Headers",
                    Reason = $"Missing {CustomHeaderOptions.XDeviceFingerprint.ToString()} header value."
                });
            }

            if (errors.Count != 0) return (false, errors);

            return (true, []);
        }

        public async Task<UserDevice?> GetRequestingDevice(string userId, string deviceFingerprint, string userAgent)
        {
            var deviceId = _deviceIdentification.GenerateDeviceId(userId, userAgent, deviceFingerprint);

            return await _userDeviceStore.GetUserDeviceByIdAsync(deviceId);
        }
    }
}
