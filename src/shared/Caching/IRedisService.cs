namespace Caching
{
    public interface IRedisService
    {
        /// <summary>
        /// Stores a raw string value in Redis using the specified key.
        /// </summary>
        /// <param name="key">The key under which the value will be stored.</param>
        /// <param name="value">The plain string value to store.</param>
        Task SetStringAsync(string key, string value);

        /// <summary>
        /// Serializes an object of type <typeparamref name="T"/> to JSON and stores it in Redis under the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize and store.</typeparam>
        /// <param name="key">The key under which the serialized value will be stored.</param>
        /// <param name="value">The object to serialize and store.</param>
        Task SetStringAsync<T>(string key, T value);

        /// <summary>
        /// Retrieves a raw string value from Redis by the specified key.
        /// </summary>
        /// <param name="key">The key associated with the stored value.</param>
        /// <returns>The string value, or <c>null</c> if the key does not exist.</returns>
        Task<string?> GetStringAsync(string key);

        /// <summary>
        /// Retrieves a value from Redis by the specified key and deserializes it into an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the stored value, which must match the shape used during serialization.</typeparam>
        /// <param name="key">The key associated with the stored value.</param>
        /// <returns>The deserialized object of type <typeparamref name="T"/>, or <c>null</c> if the key does not exist or deserialization fails.</returns>
        Task<T?> GetStringAsync<T>(string key);

        /// <summary>
        /// Deletes the specified key and its associated value from Redis.
        /// </summary>
        /// <param name="key">The key to delete.</param>
        /// <returns><c>true</c> if the key was removed; otherwise, <c>false</c>.</returns>
        Task<bool> RemoveKeyAsync(string key);

    }
}
