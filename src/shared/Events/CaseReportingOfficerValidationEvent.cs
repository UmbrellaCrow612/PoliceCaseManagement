namespace Events
{
    /// <summary>
    /// Event fired off when a case is created with a offcier ID and then it valdates that that offcie exists and then fires off if that officer exists for the given case id
    /// </summary>
    public class CaseReportingOfficerValidationEvent
    {
        public required string CaseId { get; set; }
        public required bool Exists { get; set; }
    }
}
