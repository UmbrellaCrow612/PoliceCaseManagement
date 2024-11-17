using Email.Service.Models;

namespace Email.Service.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
        Task SendBulkAsync(IEnumerable<EmailMessage> messages, CancellationToken cancellationToken = default);
    }
}
