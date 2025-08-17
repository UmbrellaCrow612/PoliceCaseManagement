namespace Identity.Core.ValueObjects
{
    /// <summary>
    /// Contains all requesting device information
    /// </summary>
    public class DeviceInfo
    {
        /// <summary>
        /// The IP address of the device
        /// </summary>
        public required string IpAddress { get; set; }

        /// <summary>
        /// User agent of the device
        /// </summary>
        public required string UserAgent { get; set; }

        /// <summary>
        /// A ID generated client side - can be any string 
        /// value considered a finger print of the browser assume it is not unique but extra information not obtainable from the server
        /// </summary>
        public required string DeviceFingerPrint { get; set; }
    }
}
