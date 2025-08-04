using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Cache.Abstractions;
using StackExchange.Redis;

namespace Cache.Redis
{
    public static class Extensions
    {
        /// <summary>
        /// Adds redis as the underlying <see cref="ICache"/> implementation
        /// </summary>
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<RedisSettings>()
             .Bind(configuration.GetSection("RedisSettings"))
             .ValidateDataAnnotations()
             .ValidateOnStart();

            var settings = configuration.GetSection("RedisSettings").Get<RedisSettings>() ?? throw new ApplicationException("Redis settings not in config");

            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { settings.Connection },
                Password = settings.Password,
            };

            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configurationOptions));

            services.AddSingleton<ICache, RedisImpl>();

            return services;
        }
    }
}
