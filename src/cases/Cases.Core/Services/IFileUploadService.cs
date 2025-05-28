namespace Cases.Core.Services
{
    /// <summary>
    /// Service for uploading, downloading, and deleting files
    /// </summary>
    public interface IFileUploadService
    {
        /// <summary>
        /// Uploads a file and returns its key (ID).
        /// </summary>
        /// <param name="fileStream">The file data stream.</param>
        /// <param name="fileName">The original file name.</param>
        Task<string> UploadFileAsync(Stream fileStream, string fileName);

        /// <summary>
        /// Deletes a file by its key (ID).
        /// </summary>
        /// <param name="fileId">The key of the file to delete.</param>
        Task DeleteFileAsync(string fileId);

        /// <summary>
        /// Downloads a file by its key (ID) and returns its stream.
        /// </summary>
        /// <param name="fileId">The key of the file to download.</param>
        Task<Stream> DownloadFileAsync(string fileId);

        /// <summary>
        /// Generates a pre-signed URL for downloading a file from S3.
        /// </summary>
        /// <param name="fileId">The key of the file.</param>
        /// <param name="expiryMinutes">Time until the URL expires (default is 10 minutes).</param>
        string GetPreSignedUrl(string fileId, int expiryMinutes = 10);
    }
}
