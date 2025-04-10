using Cases.Core.Models.Joins;

namespace Cases.Core.Models
{
    /// <summary>
    /// Represents the core information about a police case.
    /// </summary>
    public class Case
    {
        /// <summary>
        /// Unique identifier for the case (e.g., a GUID).
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        // Human-readable case number assigned by the system or department(e.g., "2023-001234").
        /// Might be assigned later than creation.
        /// </summary>
        public string? CaseNumber { get; set; } = null;

        /// <summary>
        /// A brief summary or title for the case.
        /// </summary>
        public string? Summary { get; set; } = null;

        /// <summary>
        /// Detailed description or narrative of the incident.
        /// </summary>
        public string? Description { get; set; } = null;

        /// <summary>
        /// The date and time when the incident occurred or began.
        /// </summary>
        public required DateTime IncidentDateTime { get; set; }

        /// <summary>
        /// The date and time when the incident was reported to the authorities.
        /// Defaults to the time of creation.
        /// </summary>
        public DateTime ReportedDateTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The current status of the case.
        /// </summary>
        public CaseStatus Status { get; set; } = CaseStatus.Reported;

        /// <summary>
        /// The priority level assigned to the case.
        /// </summary>
        public CasePriority Priority { get; set; } = CasePriority.Normal;


        // --- Relationships (using IDs for simplicity, could be full objects later) ---

        public ICollection<CaseIncidentType> CaseIncidentType { get; set; } = [];


        // --- Audit Info ---

        /// <summary>
        /// The date and time when this case record was created in the system.
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The date and time when this case record was last modified.
        /// </summary>
        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;
    }


    // --- Supporting Enums (Define outside the Case class but within the namespace) ---

    /// <summary>
    /// Defines the possible statuses of a case during its lifecycle.
    /// </summary>
    public enum CaseStatus
    {
        Reported,       // Initial report taken
        PendingReview,  // Waiting for assignment or initial assessment
        Active,         // Under active investigation
        Suspended,      // Investigation paused (e.g., lack of leads)
        WarrantIssued,
        Referred,       // Sent to another agency/department
        ClosedCleared,  // Solved/Arrest made/Resolved
        ClosedUnfounded,// Determined incident didn't occur as reported
        ClosedUnsolved, // Investigation concluded without resolution
        Archived
    }

    /// <summary>
    /// Defines the priority level for handling a case.
    /// </summary>
    public enum CasePriority
    {
        Low,
        Normal,
        High,
        Urgent,
        Critical
    }
}