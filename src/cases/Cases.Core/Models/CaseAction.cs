using Events.Core;

namespace Cases.Core.Models
{
    /// <summary>
    /// Represents a action taken on a <see cref="Case"/>
    /// </summary>
    public class CaseAction : IDenormalizedEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Description { get; set; }
        public required string? Notes { get; set; } = null;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        [DenormalizedField("Application user", "Id", "Identity Service")]
        public string CreatedById { get; set; } = null!;

        [DenormalizedField("Application user", "UserName", "Identity Service")]
        public string CreatedByName { get; set; } = null!;

        [DenormalizedField("Application user", "Email", "Identity Service")]
        public string CreatedByEmail { get; set; } = null!;

        /// <summary>
        /// Ef core navigation properties
        /// </summary>
        public Case Case { get; set; } = null!;

        /// <summary>
        /// Must be set
        /// </summary>
        public string CaseId { get; set; } = null!;
    }
}
