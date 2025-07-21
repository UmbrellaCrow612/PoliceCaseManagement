using Evidence.Core.Models;

namespace Evidence.API.DTOs
{
    public class EvidenceDto
    {
        public required string? Description { get; set; } = null;
        public required string ReferenceNumber { get; set; }
        public required string FileName { get; set; }
        public required string ContentType { get; set; }
        public required long FileSize { get; set; }
        public required DateTime UploadedAt { get; set; }
        public required DateTime CollectionDate { get; set; }
        public required FileUploadStatus FileUploadStatus { get; set; }
        public required string UploadedById { get; set; }
        public required string UploadedByUsername { get; set; }
        public required string UploadedByEmail { get; set; }
    }
}
