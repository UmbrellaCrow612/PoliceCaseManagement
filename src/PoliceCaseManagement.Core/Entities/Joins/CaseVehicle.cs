namespace PoliceCaseManagement.Core.Entities.Joins
{
    /// <summary>
    /// Join between <see cref="Entities.Case"/> and <see cref="Entities.Vehicle"/>
    /// </summary>
    public class CaseVehicle
    {
        public required string CaseId { get; set; }
        public required string VehicleId { get; set; }

        public Case? Case { get; set; } = null;
        public Vehicle? Vehicle { get; set; } = null;
    }
}
