namespace Events
{
    /// <summary>
    /// Event fired off when a case status created by user is validated
    /// </summary>
    public class CaseActionCreatedByValidationEvent
    {
        public required string CaseActionId { get; set; }
        public required bool CreatedByUserExists { get; set; }
    }
}
