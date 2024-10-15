namespace PoliceCaseManagement.Core.Entities
{
    /// <summary>
    /// Entity
    /// </summary>
    public class Location
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Name { get; set; }
        public required string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; } = null;
        public required string Town { get; set; }
        public required string County { get; set; }
        public required string Postcode { get; set; }
        public double? Latitude { get; set; } = null;
        public double? Longitude { get; set; } = null;
        public string? Description { get; set; } = null;

        public ICollection<Property> Properties { get; set; } = [];
        public ICollection<CrimeScene> CrimeScenes { get; set; } = [];
        public ICollection<Incident> Incidents { get; set; } = [];
    }
}
