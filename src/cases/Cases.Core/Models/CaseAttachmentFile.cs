﻿namespace Cases.Core.Models
{
    /// <summary>
    /// Stores the meta data of a cases file attachment 
    /// </summary>
    public class CaseAttachmentFile : ISoftDelete
    {
        public required string Id { get; set; }

        public required string FileName { get; set; }

        public required string S3Key { get; set; }

        public required string BucketName { get; set; }

        public required string ContentType { get; set; }

        public required long FileSize { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;


        public FileUploadStatus FileUploadStatus { get; set; } = FileUploadStatus.Failed;


        public required string CaseId { get; set; }
        public Case Case { get; set; } = null!;


        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; } = null;


        /// <summary>
        /// Marks the model as deleted
        /// </summary>
        public void Delete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }

        public void MarkUploadComplete()
        {
            FileUploadStatus = FileUploadStatus.Uploaded;
        }

        public bool IsAlreadyUploadCompleted()
        {
            return FileUploadStatus == FileUploadStatus.Uploaded;
        }
    }

    /// <summary>
    /// Enum to represent the state that a file is in
    /// </summary>
    public enum FileUploadStatus
    {
        Uploaded = 1,
        Failed = 2,
    }
}
