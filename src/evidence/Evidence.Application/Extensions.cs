using Events.Core;
using Evidence.Application.Implementations;
using Evidence.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StorageProvider.AWS;
using User.V1;

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

            services.AddGrpcClient<UserService.UserServiceClient>(o =>
            {
                o.Address = new Uri("https://localhost:7058");
            });
            services.AddScoped<UserValidationService>();

            return services;
        }
    }
}
