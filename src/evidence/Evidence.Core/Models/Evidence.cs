using Events.Core;
using Evidence.Core.Models.Joins;
using Evidence.Core.ValueObjects;
using SoftDelete.Abstractions;

namespace Evidence.Core.Models
{
    /// <summary>
    /// Represents a piece of evidence in the system
    /// </summary>
    public class Evidence : IDenormalizedEntity, ISoftDelete
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Optional Extra information about the evidence 
        /// </summary>
        public required string? Description { get; set; } = null;

        /// <summary>
        /// A unique identifier the evidence
        /// </summary>
        public required string ReferenceNumber { get; set; }

        /// <summary>
        /// Name of the file
        /// </summary>
        public required string FileName { get; set; }

        /// <summary>
        /// Amazon s3 key
        /// </summary>
        public required string S3Key { get; set; }

        /// <summary>
        /// Amazon s3 bucket it was uploaded to
        /// </summary>
        public required string BucketName { get; set; }

        /// <summary>
        /// Type of the content in the file
        /// </summary>
        public required string ContentType { get; set; }

        /// <summary>
        /// How large the file size
        /// </summary>
        public required long FileSize { get; set; }

        /// <summary>
        /// When it was uploaded to our server
        /// </summary>
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// When the piece of evidence was collected
        /// </summary>
        public required DateTime CollectionDate {  get; set; }

        /// <summary>
        /// The status the file is currently in
        /// </summary>
        public FileUploadStatus FileUploadStatus { get; set; } = FileUploadStatus.Failed;



        // delete fields
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; } = null;
        public string? DeletedById { get; set; } = null;



        // Denorm data for user
        [DenormalizedField("Application user", "Id", "Identity Service")]
        public required string UploadedById { get; set; }

        [DenormalizedField("Application user", "UserName", "Identity Service")]
        public required string UploadedByUsername { get; set; }

        [DenormalizedField("Application user", "Email", "Identity Service")]
        public required string UploadedByEmail { get; set; }



        /// <summary>
        /// Many to Many link join table between <see cref="Models.Evidence"/> and <see cref="Models.Tag"/>
        /// </summary>
        public ICollection<EvidenceTag> EvidenceTags { get; set; } = [];



        public void Delete(string deletedById)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedById = deletedById;
        }


        /// <summary>
        /// Helper method to check if the current evidence item is already marked as uploaded
        /// </summary>
        /// <returns></returns>
        public bool IsAlreadyUploaded()
        {
            return FileUploadStatus == FileUploadStatus.Uploaded;
        }

        /// <summary>
        /// Helper method to mark this evidence as uploaded
        /// </summary>
        public void MarkAsUploaded()
        {
            FileUploadStatus = FileUploadStatus.Uploaded;
        }
    }
}
