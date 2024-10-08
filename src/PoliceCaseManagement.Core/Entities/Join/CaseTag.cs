namespace PoliceCaseManagement.Core.Entities.Join
{
    public class CaseTag
    {
        public required string TagId { get; set; }
        public required string CaseId { get; set; }

        public Tag? Tag { get; set; } = null;
        public Case? Case { get; set; } = null;
    }
}
