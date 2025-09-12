using Email.Events.V1;
using MassTransit;

namespace Email.Worker.Consumers
{
    internal class EmailRequestConsumer(ILogger<EmailRequestConsumer> logger) : IConsumer<EmailRequestEvent>
    {
        public async Task Consume(ConsumeContext<EmailRequestEvent> context)
        {
            // this would not be here in prod but using email service 
              logger.LogInformation("Email sent to {to} body: {body}", context.Message.To, context.Message.Body);
        }
    }
}
