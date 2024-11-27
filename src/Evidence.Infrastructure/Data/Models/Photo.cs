using Evidence.Infrastructure.Data.Models.Joins;

namespace Evidence.Infrastructure.Data.Models
{
    public class Photo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string FileName { get; set; }
        public required string FilePath { get; set; }
        public required string FileExtension { get; set; }
        public required string Description { get; set; }
        public required DateTime TakenAt { get; set; }
        public required string TakenBy { get; set; }
        public required string Location { get; set; }

        public ICollection<CrimeScenePhoto> CrimeScenePhotos { get; set; } = [];
        public ICollection<EvidenceItemPhoto> EvidenceItemPhotos { get; set; } = [];
    }
}
