using Cases.Core.Models;

namespace Cases.Core.ValueObjects
{
    /// <summary>
    /// Query object to search for cases
    /// </summary>
    public class SearchCasesQuery
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? CaseNumber { get; set; } = null;

        public DateTime? IncidentDateTime { get; set; } = null;

        public DateTime? ReportedDateTime { get; set; } = null;

        public CaseStatus? Status { get; set; } = null;

        public CasePriority? Priority { get; set; } = null;

        public string? ReportingOfficerId { get; set; } = null;
    }
}
