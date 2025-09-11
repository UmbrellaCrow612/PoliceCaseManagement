using Email.Events.V1;
using MassTransit;

namespace Email.Worker.Consumers
{
    internal class EmailRequestConsumer(ILogger<EmailRequestConsumer> logger) : IConsumer<EmailRequestEvent>
    {
        public async Task Consume(ConsumeContext<EmailRequestEvent> context)
        {
            #if DEBUG
            logger.LogInformation($"Email sent to {context.Message.To}");
            #endif
        }
    }
}
