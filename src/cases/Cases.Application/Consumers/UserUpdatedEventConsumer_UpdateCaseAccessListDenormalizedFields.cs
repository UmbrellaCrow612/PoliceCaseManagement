using Cases.Core.Models;
using Events.Core;
using Events.User;
using MassTransit;

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
