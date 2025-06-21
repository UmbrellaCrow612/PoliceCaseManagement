namespace Cases.API.DTOs
{
    public class UploadCaseAttachmentFileResponse
    {
        public required string UploadUrl {  get; set; }
        public required string FileId { get; set; }
    }
}
