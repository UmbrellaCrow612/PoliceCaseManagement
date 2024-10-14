namespace PoliceCaseManagement.Core.Entities.Joins
{
    /// <summary>
    /// Join between a <see cref="Entities.CrimeScene"/> and <see cref="Entities.Vehicle"/>
    /// </summary>
    public class CrimeSceneVehicle
    {
        public required string CrimeSceneId { get; set; }
        public required string VehicleId { get; set; }

        public CrimeScene? CrimeScene { get; set; } = null;
        public Vehicle? Vehicle { get; set; } = null;
    }
}
