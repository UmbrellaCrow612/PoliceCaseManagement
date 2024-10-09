namespace PoliceCaseManagement.Core.Entities
{
    /// <summary>
    /// Entity
    /// </summary>
    public class Location
    {
        public ICollection<Property> Properties { get; set; } = [];
    }
}
