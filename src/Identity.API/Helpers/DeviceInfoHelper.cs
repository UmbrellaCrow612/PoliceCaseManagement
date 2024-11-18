using UAParser;

namespace Identity.API.Helpers
{
    public class DeviceInfoHelper
    {
        public string GenerateDeviceId(ClientInfo clientInfo, string ipAddress)
        {
            // Create a unique device identifier based on available information
            var components = new[]
            {
            clientInfo.UA.Family,
            clientInfo.OS.Family,
            clientInfo.Device.Family,
            ipAddress
        };

            return string.Join("-", components.Where(c => !string.IsNullOrEmpty(c)));
        }

        public string DetermineDeviceType(ClientInfo clientInfo)
        {
            // Basic device type detection logic
            var device = clientInfo.Device.Family.ToLower();
            if (device.Contains("mobile") || device.Contains("phone"))
                return "Mobile";
            if (device.Contains("tablet"))
                return "Tablet";
            if (device.Contains("tv"))
                return "TV";
            return "Desktop";
        }
    }
}
