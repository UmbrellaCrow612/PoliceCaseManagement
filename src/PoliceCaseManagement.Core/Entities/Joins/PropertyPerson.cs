using PoliceCaseManagement.Core.Entities.Enums;

namespace PoliceCaseManagement.Core.Entities.Joins
{
    /// <summary>
    /// Join between a <see cref="Entities.Property"/> and <see cref="Entities.Person"/>
    /// </summary>
    public class PropertyPerson
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required PropertyPersonRole Role { get; set; }
        public required string PropertyId { get; set; }
        public Property? Property { get; set; } = null;
        public required string PersonId { get; set; }
        public Person? Person { get; set; } = null;
    }
}
