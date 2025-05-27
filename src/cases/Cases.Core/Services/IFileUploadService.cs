namespace Cases.Core.Services
{
    /// <summary>
    /// Service for uploading and deleting files
    /// </summary>
    public interface IFileUploadService
    {
        /// <summary>
        /// Uploads a file to and returns its key (ID).
        /// </summary>
        /// <param name="fileStream">The file data stream.</param>
        /// <param name="fileName">The original file name.</param>
        Task<string> UploadFileAsync(Stream fileStream, string fileName);

        /// <summary>
        /// Deletes a file from by its key (ID).
        /// </summary>
        /// <param name="fileId">The key of the file to delete.</param>
        Task DeleteFileAsync(string fileId);
    }
}
