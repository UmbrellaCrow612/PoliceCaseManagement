namespace Cases.API.DTOs
{
    public class UploadCaseAttachmentFileResponse
    {
        public required string DownloadUrl {  get; set; }
        public required string FileId { get; set; }
    }
}
