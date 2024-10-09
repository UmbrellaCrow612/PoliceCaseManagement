namespace PoliceCaseManagement.Core.Entities.Joins
{
    /// <summary>
    /// Join between a <see cref="Entities.Incident"/> and <see cref="Entities.Person"/>
    /// </summary>
    public class IncidentPerson
    {
        public required string PersonId { get; set; }
        public required string IncidentId { get; set; }

        public Person? Person { get; set; } = null;
        public Incident? Incident { get; set; } = null;
    }
}
