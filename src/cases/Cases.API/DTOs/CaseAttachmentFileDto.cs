namespace Cases.API.DTOs
{
    public class CaseAttachmentFileDto
    {
        public required string Id { get; set; }

        public required string FileName { get; set; }

        public required string ContentType { get; set; }

        public required long FileSize { get; set; }

        public required DateTime UploadedAt { get; set; } 
    }
}
