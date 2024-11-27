namespace Evidence.Infrastructure.Data.Models.Joins
{
    public class EvidenceItemDocument
    {
        public required string EvidenceItemId { get; set; }
        public required string DocumentId { get; set; }

        public EvidenceItem? Evidence { get; set; } = null;
        public Document? Document { get; set; } = null;
    }
}
