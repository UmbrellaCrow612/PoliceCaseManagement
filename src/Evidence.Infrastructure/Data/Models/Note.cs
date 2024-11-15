namespace Evidence.Infrastructure.Data.Models
{
    public class Note
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string EvidenceItemId { get; set; }
        public EvidenceItem? Evidence { get; set; } = null;
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required DateTime CreatedAt { get; set; }
    }
}
