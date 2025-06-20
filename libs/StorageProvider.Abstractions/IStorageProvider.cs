namespace StorageProvider.Abstractions
{
    /// <summary>
    /// Defines an abstraction for cloud-based file storage. 
    /// Implement this interface in a platform-specific library such as AWS or Azure.
    /// </summary>
    public interface IStorageProvider
    {
        /// <summary>
        /// Generates a pre-signed URL to upload a file.
        /// </summary>
        /// <param name="filePath">The path (or key) where the file will be stored.</param>
        /// <param name="expirationInMinutes">Time in minutes until the URL expires.</param>
        /// <returns>A URL that allows uploading a file directly to storage.</returns>
        Task<string> GetPreSignedUploadUrlAsync(string filePath, int expirationInMinutes);

        /// <summary>
        /// Generates a pre-signed URL to download a file.
        /// </summary>
        /// <param name="filePath">The path (or key) of the file to download.</param>
        /// <param name="expirationInMinutes">Time in minutes until the URL expires.</param>
        /// <returns>A URL that allows downloading the file directly from storage.</returns>
        Task<string> GetPreSignedDownloadUrlAsync(string filePath, int expirationInMinutes);

        /// <summary>
        /// Deletes a file from storage.
        /// </summary>
        /// <param name="filePath">The path (or key) of the file to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteFileAsync(string filePath);
    }
}
