namespace Evidence.API.DTOs
{
    public class UpdateEvidenceDto
    {
        public string? Description { get; set; } = null;
        public required string FileName { get; set; }
        public required DateTime CollectionDate { get; set; }
    }
}
