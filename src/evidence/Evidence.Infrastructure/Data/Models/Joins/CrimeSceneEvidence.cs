namespace Evidence.Infrastructure.Data.Models.Joins
{
    public class CrimeSceneEvidence
    {
        public required string CrimeSceneId { get; set; }
        public required string EvidenceItemId { get; set; }

        public CrimeScene? CrimeScene { get; set; } = null;
        public EvidenceItem? EvidenceItem { get; set; } = null;
    }
}
