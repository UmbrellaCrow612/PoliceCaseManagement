using PoliceCaseManagement.Core.Entities.Enums;

namespace PoliceCaseManagement.Core.Entities.Joins
{
    /// <summary>
    /// Joins entity between a <see cref="Case"/> and a <see cref="Person"/> 
    /// </summary>
    public class CasePerson
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required CaseRole CaseRole { get; set; }
        public required string PersonId { get; set; }
        public required string CaseId { get; set; }

        public Person? Person { get; set; } = null;
        public Case? Case { get; set; } = null;
    }
}
