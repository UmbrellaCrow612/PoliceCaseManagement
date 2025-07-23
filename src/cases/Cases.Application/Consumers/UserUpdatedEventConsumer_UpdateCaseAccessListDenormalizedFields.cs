using Cases.Core.Models;
using Events.Core;
using MassTransit;
using User.Events.V1;

namespace Cases.Application.Consumers
{
    [DenormalisedEventConsumer(nameof(CaseAccessList))]
    public class UserUpdatedEventConsumer_UpdateCaseAccessListDenormalizedFields : IConsumer<UserUpdatedEvent>
    {
        public Task Consume(ConsumeContext<UserUpdatedEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
