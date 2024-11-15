namespace Evidence.Infrastructure.Data.Models
{
    public class EvidenceItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Number { get; set; }
        public required string Description { get; set; }
        public required string Type { get; set; }
        public required string CollectedBy { get; set; }
        public required DateTime CollectedAt { get; set; }
        public required string CollecationLocation { get; set; }
        public required string PhysicalDescription { get; set; }
        public required string StorageLocation { get; set; }
        public required string StorageRequirements { get; set; }
        public required bool HazmatStatus { get; set; }
        public required bool BiohazardStatus { get; set; }
        public required string Status { get; set; }
        public DateTime? DispositionDate { get; set; } = null;
        public string? DispositionMethod { get; set; } = null;
        public IEnumerable<CustodyLog> CustodyLogs { get; set; } = [];
        public IEnumerable<Photo> Photos { get; set; } = [];
        public IEnumerable<LabResult> LabResults { get; set; } = [];
        public IEnumerable<Note> Notes { get; set; } = [];
    }
}
