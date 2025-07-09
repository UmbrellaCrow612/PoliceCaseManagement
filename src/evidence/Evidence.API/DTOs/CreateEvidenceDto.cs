namespace Evidence.API.DTOs
{
    public class CreateEvidenceDto
    {
        public required string FileName { get; set; }
        public string? Description { get; set; } = null;
        public required string ReferenceNumber { get; set; }
        public required string ContentType { get; set; }
        public required long FileSize { get; set; }
        public required DateTime CollectionDate { get; set; }
    }
}
