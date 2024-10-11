using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Core.Entities
{
    /// <summary>
    /// Entity
    /// </summary>
    public class Property
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string LocationId { get; set; }
        public Location? Location { get; set; } = null;
        public required string PropertyType { get; set; }
        public string? Description { get; set; } = null;
        public string? Status { get; set; } = null;

        public ICollection<PropertyPerson> PropertyPersons { get; set; } = [];
    }
}
