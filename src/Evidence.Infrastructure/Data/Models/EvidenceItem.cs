using Evidence.Infrastructure.Data.Models.Joins;

namespace Evidence.Infrastructure.Data.Models
{
    public class EvidenceItem : ISoftDelete, IAudit
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
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
        public ICollection<CustodyLog> CustodyLogs { get; set; } = [];
        public ICollection<Photo> Photos { get; set; } = [];
        public ICollection<LabResult> LabResults { get; set; } = [];
        public ICollection<Note> Notes { get; set; } = [];
        public ICollection<CrimeSceneEvidence> CrimeSceneEvidences { get; set; } = [];

        public string? DeletedById { get; set; } = null;
        public DateTime? DeletedAt { get; set; } = null;
        public bool? IsDeleted { get; set; } = null;

        public required string CreatedById { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? LastEditedById { get; set; } = null;
        public DateTime? LastEditedAt { get; set; } = null;
    }
}
