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
        /// </summary>
        public required string CaseNumber { get; set; }

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

        /// <summary>
        /// The ID of the application user who is the reporting officer of the case.
        /// No linked through ef core nav properties as there in diff projects
        /// </summary>
        public required string ReportingOfficerId { get; set; }


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
        Reported = 0,       // Initial report taken
        PendingReview = 1,  // Waiting for assignment or initial assessment
        Active = 2,         // Under active investigation
        Suspended = 3,      // Investigation paused (e.g., lack of leads)
        WarrantIssued = 4,
        Referred = 5,       // Sent to another agency/department
        ClosedCleared = 6,  // Solved/Arrest made/Resolved
        ClosedUnfounded = 7,// Determined incident didn't occur as reported
        ClosedUnsolved = 8, // Investigation concluded without resolution
        Archived = 9
    }

    /// <summary>
    /// Defines the priority level for handling a case.
    /// </summary>
    public enum CasePriority
    {
        Low = 0,
        Normal = 1,
        High = 2,
        Urgent = 3,
        Critical = 4
    }
}