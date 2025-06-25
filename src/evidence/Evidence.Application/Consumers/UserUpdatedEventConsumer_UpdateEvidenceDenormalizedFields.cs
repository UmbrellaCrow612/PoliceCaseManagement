using Events.Core;

namespace Evidence.Application.Consumers
{
    [DenormalisedEventConsumer(nameof(Core.Models.Evidence))]
    public class UserUpdatedEventConsumer_UpdateEvidenceDenormalizedFields
    {
    }
}
