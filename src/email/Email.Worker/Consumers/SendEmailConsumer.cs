using Email.Worker.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using MassTransit;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Email.Worker.Consumers
{
    public class SendEmailConsumer(IOptions<EmailSettings> options, ILogger<SendEmailConsumer> logger) : IConsumer<SendEmailEvent>
    {
        private readonly EmailSettings _emailSettings = options.Value;
        private readonly ILogger<SendEmailConsumer> _logger = logger;

        public async Task Consume(ConsumeContext<SendEmailEvent> context)
        {
            _logger.LogInformation("SendEmailConsumer started");

            using var message = new MimeMessage();
            message.From.Add(new MailboxAddress(
                "Yousaf",
                _emailSettings.From
            ));

            message.To.Add(new MailboxAddress(
            "Rec",
            context.Message.To
            ));

            message.Subject = "Sending with Twilio SendGrid is Fun";

            var bodyBuilder = new BodyBuilder
            {
                TextBody = "and easy to do anywhere, especially with C#",
                HtmlBody = "and easy to do anywhere, <b>especially with C#</b>"
            };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_emailSettings.SmtpHost, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(
                userName: _emailSettings.Username,
                password: _emailSettings.Password
            );
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            _logger.LogInformation("SendEmailConsumer finished");
        }
    }
}
