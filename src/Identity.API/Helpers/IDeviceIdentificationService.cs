namespace Identity.API.Helpers
{
    public interface IDeviceIdentificationService
    {
        string GenerateDeviceId(string ipAddress, string userAgent);
    }
}
