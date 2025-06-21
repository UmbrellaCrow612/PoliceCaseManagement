@ -0,0 +1,55 @@
﻿using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using StorageProvider.Abstractions;

namespace StorageProvider.AWS
{
    /// <summary>
    /// Internal AWS implementation
    /// </summary>
    public class AWSImpl(IOptions<AWSSettings> options, IAmazonS3 amazonS3) : IStorageProvider
    {
        private readonly AWSSettings _settings = options.Value;
        private readonly IAmazonS3 _amazonS3 = amazonS3;

        public async Task DeleteFileAsync(string filePath)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = filePath
            };

            await _amazonS3.DeleteObjectAsync(request);
        }

        public Task<string> GetPreSignedDownloadUrlAsync(string filePath, int? expirationInMinutes = 5)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _settings.BucketName,
                Key = filePath,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddMinutes((double)expirationInMinutes)
            };

            return Task.FromResult(_amazonS3.GetPreSignedURL(request));
        }

        public Task<string> GetPreSignedUploadUrlAsync(string filePath, string contentType, int? expirationInMinutes = 5)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _settings.BucketName,
                Key = filePath,
                ContentType = contentType,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddMinutes((double)expirationInMinutes),

            };

            return Task.FromResult(_amazonS3.GetPreSignedURL(request));
        }
    }
}
