namespace PoliceCaseManagement.Core.Entities
{
    /// <summary>
    /// Entity
    /// </summary>
    public class Location
    {
        public ICollection<Property> Properties { get; set; } = [];
        public ICollection<CrimeScene> CrimeScenes { get; set; } = [];    
    }
}
