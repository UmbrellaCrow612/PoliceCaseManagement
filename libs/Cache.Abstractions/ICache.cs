namespace Cache.Abstractions
{
    /// <summary>
    /// Represents a distributed cache that can be used to store or retrieve key-value pairs.
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Retrieves a cached item by key.
        /// </summary>
        /// <typeparam name="T">The type of the cached item.</typeparam>
        /// <param name="key">The unique key of the cached item.</param>
        /// <returns>The cached item, or null if not found.</returns>
        Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets a value in the cache with an optional expiration time.
        /// </summary>
        /// <typeparam name="T">The type of the value to cache.</typeparam>
        /// <param name="key">The unique key for the value.</param>
        /// <param name="value">The value to cache.</param>
        /// <param name="expiration">Optional expiration time.</param>
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a value from the cache by key.
        /// </summary>
        /// <param name="key">The key of the value to remove.</param>
        Task DeleteAsync(string key, CancellationToken cancellationToken = default);
    }
}
