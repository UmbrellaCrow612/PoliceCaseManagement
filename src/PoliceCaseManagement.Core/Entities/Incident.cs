using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Core.Entities
{
    /// <summary>
    /// Entity
    /// </summary>
    public class Incident
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string IncidentType { get; set; }
        public required string Details { get; set; }
        public required string LocationId { get; set; }
        public Location? Location { get; set; } = null;

        public ICollection<IncidentPerson> IncidentPersons { get; set; } = [];
    }
}
