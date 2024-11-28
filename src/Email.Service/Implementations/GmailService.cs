using Email.Service.Interfaces;
using Email.Service.Models;

namespace Email.Service.Implamentations
{
    public class GmailService : IEmailService
    {
        public Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task SendBulkAsync(IEnumerable<EmailMessage> messages, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
