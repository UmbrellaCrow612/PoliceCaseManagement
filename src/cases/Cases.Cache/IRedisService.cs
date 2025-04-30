namespace Cases.Cache
{
    public interface IRedisService
    {
        Task SetStringAsync(string key, string value);
        Task<string?> GetStringAsync(string key);
    }
}
