namespace Events
{
    /// <summary>
    /// Event fired off when a case is created
    /// </summary>
    public class CaseCreatedEvent
    {
        /// <summary>
        /// The cases ID
        /// </summary>
        public required string CaseId { get; set; }

        /// <summary>
        /// The reporting offcier id created with the case - this is fetched from the identity api typically and shown in ui then sent across 
        /// in creation
        /// </summary>
        public required string ReportingOffcierId { get; set; }
    }
}
