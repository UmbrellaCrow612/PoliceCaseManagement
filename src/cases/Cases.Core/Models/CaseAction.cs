namespace Cases.Core.Models
{
    /// <summary>
    /// Represents a action taken on a <see cref="Case"/>
    /// </summary>
    public class CaseAction
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Description { get; set; }
        public string? Notes { get; set; } = null;

        /// <summary>
        /// The <see cref="Case.Status"/> at the time of this action being recorded
        /// </summary>
        public required CaseStatus CaseStatus { get; set; }

        /// <summary>
        /// Indicates if the case action is in a valid state by default it is pending as the created by ID needs to be validated
        /// </summary>
        public CaseActionValidationStatus ValidationStatus { get; set; } = CaseActionValidationStatus.Pending;


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// User id of the person who created it
        /// </summary>
        public required string CreatedById { get; set; }


        /// <summary>
        /// Ef core navigation properties
        /// </summary>
        public required Case Case { get; set; } = null!;
        public required string CaseId { get; set; }
    }

    /// <summary>
    /// Enum representing all the types of <see cref="CaseAction"/> that can be recorded
    /// </summary>
    public enum CaseActionTypes
    {
        NoteAdded = 0,
        OfficerAssigned = 1,
        StatusChanged = 2,
        EvidenceLogged = 3,
        SuspectInterviewed = 4,
        // add more as needed
    }

    /// <summary>
    /// Indicates if a case action is in a valid state
    /// </summary>
    public enum CaseActionValidationStatus
    {
        Pending = 0,
        Valid = 1,
        Invalid = 3
    }
}
