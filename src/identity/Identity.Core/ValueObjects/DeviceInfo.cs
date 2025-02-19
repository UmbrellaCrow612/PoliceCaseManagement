namespace Identity.Core.ValueObjects
{
    /// <summary>
    /// Requesting device info mapped from a http request so we are not bound to api concerns
    /// </summary>
    public class DeviceInfo
    {
        public required string IpAddress { get; set; }
        public required string UserAgent { get; set; }
        public required string DeviceFingerPrint { get; set; }
    }
}
