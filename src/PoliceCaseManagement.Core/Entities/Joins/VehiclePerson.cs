using PoliceCaseManagement.Core.Entities.Enums;

namespace PoliceCaseManagement.Core.Entities.Joins
{
    /// <summary>
    /// Join between a <see cref="Entities.Vehicle"/> and <see cref="Entities.Person"/>
    /// </summary>
    public class VehiclePerson
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string VehicleId { get; set; }
        public required string PersonId { get; set; }
        public required VehiclePersonRole Role { get; set; }

        public Vehicle? Vehicle { get; set; } = null;
        public Person? Person { get; set; } = null;
    }
}
