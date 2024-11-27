using Evidence.Infrastructure.Data.Models.Joins;

namespace Evidence.Infrastructure.Data.Models
{
    public class CrimeScene
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required DateTime DateReported { get; set; }
        public required string Location { get; set; }
        public required string Description { get; set; }
        public required string Type { get; set; }

        public ICollection<CrimeSceneEvidence> CrimeSceneEvidences { get; set; } = [];
    }
}
