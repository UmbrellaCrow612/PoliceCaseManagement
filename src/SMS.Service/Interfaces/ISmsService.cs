namespace SMS.Service.Interfaces
{
    internal interface ISmsService
    {
        Task<bool> SendSmsAsync(string toPhoneNumber, string message);
    }
}
