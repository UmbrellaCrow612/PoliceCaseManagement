using MassTransit;
using Sms.Events.V1;

namespace SMS.Worker.Consumers
{
    internal class SmsRequestEventConsumer(ILogger<SmsRequestEventConsumer> logger) : IConsumer<SmsRequestEvent>
    {
        public async Task Consume(ConsumeContext<SmsRequestEvent> context)
        {
            // use real impl
            logger.LogInformation("Event fired off for {id} with message {mess}", context.Message.RequestId, context.Message.Message);
        }
    }
}
