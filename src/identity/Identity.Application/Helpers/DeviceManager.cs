using Identity.Application.Codes;
using Identity.Application.Constants;
using Identity.Core.Models;
using Identity.Core.Repositorys;
using Identity.Core.Services;
using Microsoft.AspNetCore.Http;
using UAParser;


namespace Identity.Application.Helpers
{
    public class DeviceManager(IDeviceIdentification deviceIdentification, IUnitOfWork unitOfWork)
    {
        private readonly IDeviceIdentification _deviceIdentification = deviceIdentification;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

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
        public static bool UserAgentValid(string userAgent)
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

        private class Err : IServiceError
        {
            public required string Code { get; set; }
            public string? Message { get; set; }
        }

        public static (bool isValid, List<IServiceError> errors) VerifyRequestHasRequiredProperties(HttpRequest request)
        {
            List<IServiceError> errors = [];

            string userAgent = request.Headers["User-Agent"].ToString();
            var deviceFingerprint = request.Headers[CustomHeaderOptions.XDeviceFingerprint].FirstOrDefault();

            if (!UserAgentValid(userAgent))
            {
                errors.Add(new Err
                {
                    Code = BusinessRuleCodes.ValidationError,
                    Message = "User agent malformed"
                });
            }

            if (string.IsNullOrWhiteSpace(deviceFingerprint))
            {
                errors.Add(new Err
                {
                    Code = BusinessRuleCodes.DeviceFingerprintMissing,
                    Message = "Device fingerprint missing"
                });
            }

            if (errors.Count != 0) return (false, errors);

            return (true, []);
        }

        public async Task<UserDevice?> GetRequestingDevice(string userId, string deviceFingerprint, string userAgent)
        {
            var deviceId = _deviceIdentification.GenerateDeviceId(userId, userAgent, deviceFingerprint);

            return await _unitOfWork.Repository<UserDevice>().FindByIdAsync(deviceId);
        }

        public string GenerateDeviceId(string userId, string deviceFingerprint, string userAgent)
        {
            return _deviceIdentification.GenerateDeviceId(userId, userAgent, deviceFingerprint);
        }
    }
}
