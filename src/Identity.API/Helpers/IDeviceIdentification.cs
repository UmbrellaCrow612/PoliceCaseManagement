﻿namespace Identity.API.Helpers
{
    public interface IDeviceIdentification
    {
        /// <summary>
        /// Generates a unique device identifier based on user and device information.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who has provided valid credentials. Must not be null or empty.</param>
        /// <param name="userAgent">The user agent string containing device and browser information. Must not be null or empty.</param>
        /// <returns>A unique hash string representing the device, combining user ID and client information.</returns>
        /// <exception cref="ArgumentException">Thrown when either userId or userAgent is null, empty, or consists only of white-space characters.</exception>
        /// <remarks>
        /// The device ID is generated by combining and hashing the following components:
        /// - User Agent family
        /// - User Agent major version
        /// - Operating System family
        /// - Operating System major version
        /// - Device family
        /// - User ID
        /// </remarks>
        string GenerateDeviceId(string userId, string userAgent);
    }
}
