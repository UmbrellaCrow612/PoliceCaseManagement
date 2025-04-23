namespace Cases.API.DTOs
{
    public class CreateCaseDto
    {
        public required string CaseNumber { get; set; }
        public required string? Summary { get; set; } = null;
        public required string? Description { get; set; } = null;

        public required DateTime IncidentDateTime { get; set; }

        public required string ReportingOfficerId { get; set; }
    }
}
