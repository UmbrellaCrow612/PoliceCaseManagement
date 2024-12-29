using Email.Service.Interfaces;
using Email.Service.Models;
using Email.Service.Settings;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Email.Service.Implamentations
{
    public class GmailService(IOptions<EmailSettings> emailSettings) : IEmailService
    {
        private readonly EmailSettings _emailSettings = emailSettings.Value;

        public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
        {
            using var client = CreateSmtpClient();
            var mailMessage = CreateMailMessage(message);

            await client.SendMailAsync(mailMessage, cancellationToken);
        }

        private SmtpClient CreateSmtpClient()
        {
            return new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword)
            };
        }

        private MailMessage CreateMailMessage(EmailMessage message)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail),
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = message.IsHtml
            };

            mailMessage.To.Add(message.To);

            return mailMessage;
        }
    }
}
