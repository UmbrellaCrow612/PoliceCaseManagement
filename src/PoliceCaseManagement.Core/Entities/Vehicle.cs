using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Core.Entities
{
    /// <summary>
    /// Entity
    /// </summary>
    public class Vehicle
    {
        public required string Id { get; set; } = Guid.NewGuid().ToString();
        public required string LicensePlate { get; set; }
        public required string Make { get; set; } 
        public required string Model { get; set; } 
        public required int Year { get; set; } 
        public required string Color { get; set; } 
        public required string VIN { get; set; }

        public ICollection<VehiclePerson> VehiclePersons { get; set; } = [];
        public ICollection<CaseVehicle> CaseVehicles { get; set; } = [];
        public ICollection<CrimeSceneVehicle> CrimeSceneVehicles { get; set; } = [];
    }
}
