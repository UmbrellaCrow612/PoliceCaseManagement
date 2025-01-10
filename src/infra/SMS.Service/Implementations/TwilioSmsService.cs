using Microsoft.Extensions.Options;
using SMS.Service.Interfaces;
using SMS.Service.Models;
using SMS.Service.Settings;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;

namespace SMS.Service.Implementations
{
    public class TwilioSmsService : ISmsService
    {
        private readonly SmsSettings _smsSettings;

        public TwilioSmsService(IOptions<SmsSettings> smsSettings)
        {
            _smsSettings = smsSettings.Value;
            TwilioClient.Init(_smsSettings.AccountSid, _smsSettings.AuthToken);
        }

        public async Task<bool> SendSmsAsync(string toPhoneNumber, string message)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(toPhoneNumber))
                    throw new ArgumentException("Phone number cannot be empty", nameof(toPhoneNumber));

                if (string.IsNullOrWhiteSpace(message))
                    throw new ArgumentException("Message cannot be empty", nameof(message));

                // Send SMS using Twilio
                var messageResource = await MessageResource.CreateAsync(
                    body: message,
                    from: new PhoneNumber(_smsSettings.FromPhoneNumber),
                    to: new PhoneNumber(toPhoneNumber)
                );

                // Check message status
                return messageResource.Status == MessageResource.StatusEnum.Sent ||
                       messageResource.Status == MessageResource.StatusEnum.Queued;
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a proper logging framework)
                Console.WriteLine($"SMS sending failed: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendSmsAsync(SmsRequest smsRequest)
        {
            if (smsRequest == null)
                throw new ArgumentNullException(nameof(smsRequest));

            return await SendSmsAsync(smsRequest.ToPhoneNumber, smsRequest.Message);
        }
    }
}
