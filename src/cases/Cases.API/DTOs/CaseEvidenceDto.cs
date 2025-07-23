namespace Cases.API.DTOs
{
    public class CaseEvidenceDto
    {
        public required string Id { get; set; }

        public required string CaseId { get; set; }

        public required string EvidenceId { get; set; }

        public required string EvidenceName { get; set; }

        public required string EvidenceReferenceNumber { get; set; }
    }
}
