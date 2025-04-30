namespace Events
{
    /// <summary>
    /// Event which is fired off when a case action is added to a case.
    /// </summary>
    public class CaseActionCreatedEvent
    {
        public required string CaseActionId { get; set; }

        /// <summary>
        /// The user who created this cse action
        /// </summary>
        public required string CreatedById { get; set; }
    }
}
