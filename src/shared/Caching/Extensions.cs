using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Caching
{
    public static class Extensions
    {
        /// <summary>
        /// Adds Redis cache to the application - adds <see cref="RedisSettings"/> to the DI and uses those settings to add it.
        /// </summary>
        public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<RedisSettings>()
               .Bind(configuration.GetSection("RedisSettings"))
               .ValidateDataAnnotations()
               .ValidateOnStart();

            var settings = configuration.GetSection("RedisSettings").Get<RedisSettings>() ?? throw new ApplicationException("Redis settings not in config");

            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { settings.Connection },
                Password = settings.Password
            };

            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configurationOptions));
            services.AddScoped<IRedisService, RedisService>();


            return services;
        }
    }
}
