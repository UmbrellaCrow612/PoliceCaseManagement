namespace Reporting.Core.Models
{
    /// <summary>
    /// A report made by a civillian
    /// </summary>
    public class Report
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string IncidentType { get; set; }
        public required string IncidentLocation { get; set; }
        public required DateTime DateOfIncident { get; set; }
        public required TimeSpan TimeOfIncident { get; set; }
        public required string IncidentDescription { get; set; }
    }
}
