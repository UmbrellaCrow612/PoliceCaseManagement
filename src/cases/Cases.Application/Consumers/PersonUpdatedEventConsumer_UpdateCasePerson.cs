using Cases.Core.Models.Joins;
using Events.Core;

namespace Cases.Application.Consumers
{
    [DenormalisedEventConsumer(nameof(CasePerson))]
    internal class PersonUpdatedEventConsumer_UpdateCasePerson
    {
    }
}
