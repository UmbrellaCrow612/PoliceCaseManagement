namespace Cases.Core.Models
{
    /// <summary>
    /// Represents a action taken on a <see cref="Case"/>
    /// </summary>
    public class CaseAction
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Description { get; set; }
        public required string? Notes { get; set; } = null;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// User id of the person who created it
        /// </summary>
        public string CreatedById { get; set; } = null!;
        public string CreatedByName { get; set; } = null!;
        public string CreatedByEmail { get; set; } = null!;
        // Recommended Enterprise Approach: DE normalize Only What You Need and listen to events to update this locally


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
