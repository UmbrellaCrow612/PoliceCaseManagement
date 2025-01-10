namespace Evidence.Infrastructure.Data.Models
{
    public class CustodyLog
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string EvidenceItemId { get; set; }
        public EvidenceItem? Evidence { get; set; } = null;
        public required string TransferredFromName { get; set; }
        public required string TransferredToName { get; set; }
        public required DateTime TransferredAt { get; set; }
        public required string Purpose { get; set; }
        public required string Location { get; set; }
        public required string Remarks { get; set; }
    }
}
