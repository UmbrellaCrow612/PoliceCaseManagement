using Events.Core;
using Evidence.Application.Implementations;
using Evidence.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StorageProvider.AWS;

namespace Evidence.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAWSStorageProvider(configuration);

            services.AddScoped<IEvidenceService, EvidenceService>();
            services.AddScoped<ITagService, TagService>();

            services.EnsureDenormalisedEntitiesHaveAConsumer();

            services.AddScoped<UserValidationService>();

            return services;
        }
    }
}
