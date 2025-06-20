namespace Cases.API.DTOs
{
    public class UploadCaseAttachmentRequest
    {
        public required string ContentType { get; set; }
        public required string FileName { get; set; }
        public required long FileSize { get; set; }
    }
}
