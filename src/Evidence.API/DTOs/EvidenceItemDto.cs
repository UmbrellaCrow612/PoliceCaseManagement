namespace Evidence.API.DTOs
{
    public class EvidenceItemDto
    {
        public required string Id { get; set; }
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

        public string? DeletedById { get; set; } = null;
        public DateTime? DeletedAt { get; set; } = null;
        public bool? IsDeleted { get; set; } = null;

        public required string CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? LastEditedById { get; set; } = null;
        public DateTime? LastEditedAt { get; set; } = null;
    }
}
