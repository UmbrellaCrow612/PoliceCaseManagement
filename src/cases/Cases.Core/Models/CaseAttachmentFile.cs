 namespace Cases.Core.Models
{
    /// <summary>
    /// Stores the meta data of a cases file attachment 
    /// </summary>
    public class CaseAttachmentFile
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string FileName { get; set; }

        public required string S3Key { get; set; }

        public required string BucketName { get; set; }

        public required string ContentType { get; set; }

        public required long FileSize { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;


        public required string CaseId { get; set; }
        public Case Case { get; set; } = null!;
    }
}
