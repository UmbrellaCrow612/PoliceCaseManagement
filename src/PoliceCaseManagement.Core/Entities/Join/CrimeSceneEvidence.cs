namespace PoliceCaseManagement.Core.Entities.Join
{
    /// <summary>
    /// Join between a <see cref="Entities.CrimeScene"/> and <see cref="Entities.Evidence"/>
    /// </summary>
    public class CrimeSceneEvidence
    {
        public required string CrimeSceneId { get; set; }
        public required string EvidenceId { get; set; }

        public CrimeScene? CrimeScene { get; set; } = null;
        public Evidence? Evidence { get; set; } = null;
    }
}
