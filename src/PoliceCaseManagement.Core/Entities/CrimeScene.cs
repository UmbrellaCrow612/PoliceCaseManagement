using PoliceCaseManagement.Core.Entities.Interfaces;
using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Core.Entities
{
    public class CrimeScene : ISoftDeletable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Description { get; set; }
        public Location? Location { get; set; } = null;
        public required string LocationId { get; set; }

        public DateTime? DeletedAt { get; set; } = null;
        public bool IsDeleted { get; set; } = false;
        public string? DeletedById { get; set; } = null;
        public User? DeletedBy { get; set; } = null;

        public ICollection<CrimeSceneEvidence> CrimeSceneEvidences { get; set; } = [];
        public ICollection<CaseCrimeScene> CaseCrimeScenes { get; set; } = [];
    }
}
