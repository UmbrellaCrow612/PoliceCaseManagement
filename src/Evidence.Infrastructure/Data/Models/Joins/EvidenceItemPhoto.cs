namespace Evidence.Infrastructure.Data.Models.Joins
{
    public class EvidenceItemPhoto
    {
        public required string EvidenceItemId { get; set; }
        public required string PhotoId { get; set; }

        public EvidenceItem? Evidence { get; set; } = null;
        public Photo? Photo { get; set; } = null; 
    }
}
