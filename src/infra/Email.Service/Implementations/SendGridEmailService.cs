using Email.Service.Interfaces;
using Email.Service.Models;
using Email.Service.Settings;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Email.Service.Implementations
{
    internal class SendGridEmailService(IOptions<EmailSettings> options) : IEmailService
    {
        private readonly EmailSettings _settings = options.Value;

        public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
        {
            var client = new SendGridClient(_settings.SendGridSettings.APIKey);

            var email = MailHelper.CreateSingleEmail(
                from: new EmailAddress(_settings.FromEmail),
                to: new EmailAddress(message.To),
                subject: message.Subject,
                plainTextContent: message.Body,
                htmlContent: message.Body
            );

            var response = await client.SendEmailAsync(email, cancellationToken);

            // Optional: Handle response
            if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                throw new Exception($"Email send failed: {response.StatusCode}");
            }
        }
    }
}
