using System.Text.Json;
using StackExchange.Redis;

namespace Caching
{
    public class RedisService(IConnectionMultiplexer redis) : IRedisService
    {
        private readonly IDatabase _db = redis.GetDatabase();

        public async Task SetStringAsync(string key, string value)
        {
            await _db.StringSetAsync(key, value);
        }

        public async Task<string?> GetStringAsync(string key)
        {
            return await _db.StringGetAsync(key);
        }

        public async Task SetStringAsync<T>(string key, T value)
        {
            string jsonString = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, jsonString);
        }

        public async Task<T?> GetStringAsync<T>(string key)
        {
            string? value = await _db.StringGetAsync(key);
            if (string.IsNullOrWhiteSpace(value))
            {
                return default;
            }

            try
            {
                T? shaped = JsonSerializer.Deserialize<T>(value);
                return shaped;
            }
            catch (JsonException)
            {
                return default;
            }
        }

        public async Task<bool> RemoveKeyAsync(string key)
        {
            return await _db.KeyDeleteAsync(key);
        }

    }
}
