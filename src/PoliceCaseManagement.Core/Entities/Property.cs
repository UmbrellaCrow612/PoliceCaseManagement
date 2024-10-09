using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Core.Entities
{
    /// <summary>
    /// Entity
    /// </summary>
    public class Property
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();


        public ICollection<PropertyPerson> PropertyPersons { get; set; } = [];
    }
}
