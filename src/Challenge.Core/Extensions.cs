using Challenge.Core.Helpers;
using Challenge.Core.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace Challenge.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddChallenges(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<ChallengeJwtSettings>()
                 .Bind(configuration.GetSection("ChallengeJwtSettings"))
                 .ValidateDataAnnotations()
                 .ValidateOnStart();

            services.AddScoped<JwtChallengeHelper>();

            return services;
        }
    }
}
