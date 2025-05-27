using Amazon.S3;
using Amazon.S3.Model;
using Cases.Core.Services;
using Cases.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cases.Infrastructure.Implementations
{
    /// <summary>
    /// Implementation of file uploading using amazon s3
    /// </summary>
    internal class AWSs3FileUploadService(IOptions<AWSSettings> options, IAmazonS3 amazonS3) : IFileUploadService
    {
        private readonly AWSSettings _settings = options.Value;
        private readonly IAmazonS3 _s3Client = amazonS3;

        public async Task DeleteFileAsync(string fileId)
        {
            if (string.IsNullOrEmpty(fileId)) throw new ArgumentNullException(nameof(fileId));

            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = fileId
            };

            await _s3Client.DeleteObjectAsync(deleteRequest);
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            if (fileStream == null) throw new ArgumentNullException(nameof(fileStream));
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException(nameof(fileName));

            var fileKey = $"{Guid.NewGuid()}_{fileName}";

            var putRequest = new PutObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = fileKey,
                InputStream = fileStream,
                ContentType = GetContentType(fileName)
            };

            await _s3Client.PutObjectAsync(putRequest);

            return fileKey;
        }


        private static string GetContentType(string fileName)
        {
            // Basic content type detection by file extension
            var extension = Path.GetExtension(fileName).ToLowerInvariant();

            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".pdf" => "application/pdf",
                ".txt" => "text/plain",
                ".doc" or ".docx" => "application/msword",
                _ => "application/octet-stream"
            };
        }
    }
}
