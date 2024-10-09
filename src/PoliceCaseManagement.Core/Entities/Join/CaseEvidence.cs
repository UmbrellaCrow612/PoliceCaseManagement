namespace PoliceCaseManagement.Core.Entities.Join
{
    /// <summary>
    /// Join between a <see cref="Case"/> and <see cref="Evidence"/>
    /// </summary>
    public class CaseEvidence
    {
        public required string CaseId { get; set; }
        public required string EvidenceId { get; set; }

        public Case? Case { get; set; } = null;
        public Evidence? Evidence { get; set; } = null;
    }
}
