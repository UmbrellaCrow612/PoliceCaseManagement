namespace Identity.API.Helpers
{
    public interface IDeviceIdentification
    {
        string GenerateDeviceId(string ipAddress, string userAgent);
    }
}
