using Events.Core;

namespace Cases.Core.Models.Joins
{
    /// <summary>
    /// Join between a case and a piece of Evidence
    /// </summary>
    public class CaseEvidence : IDenormalizedEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// The <see cref="Cases.Core.Models.Case"/> is linked to
        /// </summary>
        public required string CaseId { get; set; }
        public Case Case { get; set; } = null!;

        /// <summary>
        /// The specific Evidence it is linked to
        /// </summary>
        [DenormalizedField("Evidence Service", "Id", "Evidence Service")]
        public required string EvidenceId { get; set; }

        /// <summary>
        /// A Name to describe what the evidence is.
        /// </summary>
        [DenormalizedField("Evidence Service", "Name", "Evidence Service")]
        public required string EvidenceName { get; set; }

        /// <summary>
        /// A ref number of the evidence.
        /// </summary>
        [DenormalizedField("Evidence Service", "ReferenceNumber", "Evidence Service")]
        public required string EvidenceReferenceNumber { get; set; }
    }
}
