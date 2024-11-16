using System.ComponentModel.DataAnnotations;

namespace Evidence.API.DTOs
{
    public class UpdateEvidenceItemDto
    {
        [Required]
        public required string Description { get; set; }

        [Required]
        public required string Type { get; set; }

        [Required]
        public required string CollectedBy { get; set; }

        [Required]
        public required DateTime CollectedAt { get; set; }

        [Required]
        public required string CollecationLocation { get; set; }

        [Required]
        public required string PhysicalDescription { get; set; }

        [Required]
        public required string StorageLocation { get; set; }

        [Required]
        public required string StorageRequirements { get; set; }

        [Required]
        public required bool HazmatStatus { get; set; }

        [Required]
        public required bool BiohazardStatus { get; set; }

        [Required]
        public required string Status { get; set; }

        public DateTime? DispositionDate { get; set; } = null;
        public string? DispositionMethod { get; set; } = null;
    }
}
