using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Core.Entities
{
    /// <summary>
    /// Entity
    /// </summary>
    public class Incident
    {
        public required string LocationId { get; set; }
        public Location? Location { get; set; } = null;

        public ICollection<IncidentPerson> IncidentPersons { get; set; } = [];
    }
}
