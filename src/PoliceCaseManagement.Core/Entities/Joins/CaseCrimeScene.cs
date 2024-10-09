namespace PoliceCaseManagement.Core.Entities.Joins
{
    /// <summary>
    /// Joins between a <see cref="Entities.Case"/> and <see cref="Entities.CrimeScene"/>
    /// </summary>
    public class CaseCrimeScene
    {
        public required string CaseId { get; set; }
        public required string CrimeSceneId { get; set; }


        public Case? Case { get; set; } = null;
        public CrimeScene? CrimeScene { get; set; } = null;
    }
}
