using Cases.Core.Models.Joins;

namespace Cases.Core.Models
{
    /// <summary>
    /// Model for a IncidentType a <see cref="Case"/> can be linked to.
    /// </summary>
    public class IncidentType
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Name for the IncidentType
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Optional description about the IncidentType
        /// </summary>
        public string? Description { get; set; } = null;


        public ICollection<CaseIncidentType> CaseIncidentType { get; set; } = [];
    }
}
