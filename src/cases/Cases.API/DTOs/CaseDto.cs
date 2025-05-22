using Cases.Core.Models;

namespace Cases.API.DTOs
{
    public class CaseDto
    {
        public required string Id { get; set; }
        public required string CaseNumber { get; set; }
        public required string? Summary { get; set; } = null;
        public required string? Description { get; set; } = null;
        public required DateTime IncidentDateTime { get; set; }
        public required DateTime ReportedDateTime { get; set; }
        public required CaseStatus Status { get; set; }
        public required CasePriority Priority { get; set; }
        public required DateTime LastModifiedDate { get; set; }
        public required string ReportingOfficerId { get; set; }
        public required string ReportingOfficerUserName { get; set; } 
        public required string ReportingOfficerEmail { get; set; } 
    }
}
