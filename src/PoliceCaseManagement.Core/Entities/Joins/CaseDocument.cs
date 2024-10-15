namespace PoliceCaseManagement.Core.Entities.Joins
{
    /// <summary>
    /// Joins entity between a <see cref="Case"/> and a <see cref="Document"/> 
    /// </summary>
    public class CaseDocument
    {
        public required string CaseId { get; set; }
        public required string DocumentId { get; set; }

        public Case? Case { get; set; } = null;
        public Document? Document { get; set; } = null;
    }
}
