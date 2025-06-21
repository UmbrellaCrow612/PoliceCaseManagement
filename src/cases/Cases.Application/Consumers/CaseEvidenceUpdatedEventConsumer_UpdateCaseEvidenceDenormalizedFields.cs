using Cases.Core.Models;
using Cases.Core.Models.Joins;
using Events.Core;

namespace Cases.Application.Consumers
{
    [DenormalisedEventConsumer(nameof(CaseEvidence))]
    public class CaseEvidenceUpdatedEventConsumer_UpdateCaseEvidenceDenormalizedFields
    {
    }
}
