namespace Evidence.Infrastructure.Data.Models
{
    public class LabResult
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string EvidenceItemId { get; set; }
        public EvidenceItem? Evidence { get; set; } = null;
        public required string TestName { get; set; }
        public required DateTime TestDate { get; set; }
        public required string TestedBy { get; set; }
        public required string Findings { get; set; }
        public required string Conclusions { get; set; }
    }
}
