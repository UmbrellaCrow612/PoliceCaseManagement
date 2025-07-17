namespace Cases.Core.ValueObjects
{
    public class CaseAttachmentFileMetaData
    {
        public required string ContentType { get; set; }
        public required string FileName { get; set; }
        public required long FileSize { get; set; }
    }
}
