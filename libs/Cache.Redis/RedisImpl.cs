using Cache.Abstractions;
using StackExchange.Redis;
using System.Text.Json;

namespace Cache.Redis
{
    /// <summary>
    /// Implementation of <see cref="ICache"/> using Redis.
    /// </summary>
    internal class RedisImpl(IConnectionMultiplexer redis) : ICache
    {
        private readonly IDatabase _db = redis.GetDatabase();

        public async Task DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            await _db.KeyDeleteAsync(key);
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var redisValue = await _db.StringGetAsync(key);
            if (redisValue.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(redisValue);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            var serialized = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, serialized, expiration);
        }
    }
}
