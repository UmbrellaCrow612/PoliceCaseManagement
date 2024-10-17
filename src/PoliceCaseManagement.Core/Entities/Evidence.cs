using PoliceCaseManagement.Core.Entities.Interfaces;
using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Core.Entities
{
    /// <summary>
    /// Entity
    /// </summary>
    public class Evidence : ISoftDeletable, IAuditable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string FileUrl { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string? ChainOfCustody { get; set; }
        public bool IsConfidential { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastEditedAt { get; set; } = null;
        public required string CreatedById { get; set; }
        public string? LastEditedById { get; set; } = null;
        public User? CreatedBy { get; set; } = null;
        public required User? LastEditedBy { get; set; } = null;

        public DateTime? DeletedAt { get; set; } = null;
        public bool IsDeleted { get; set; } = false;
        public string? DeletedById { get; set; } = null;
        public User? DeletedBy { get; set; } = null;

        public ICollection<CaseEvidence> CaseEvidences { get; set; } = [];
        public ICollection<CrimeSceneEvidence> CrimeSceneEvidences { get; set; } = [];
    }
}
