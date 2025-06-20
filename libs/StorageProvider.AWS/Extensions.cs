using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StorageProvider.Abstractions;

namespace StorageProvider.AWS
{
    public static class Extensions
    {
        /// <summary>
        /// Adds AWS implementation of the underlying storage provider
        /// </summary>
        public static IServiceCollection AddAWSStorageProvider(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<AWSSettings>()
                .Bind(configuration.GetSection("aws"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            var awsSettings = configuration.GetSection("aws").Get<AWSSettings>() ?? throw new ApplicationException("AWS Settings missing");

            var credentials = new BasicAWSCredentials(awsSettings.AccessKey, awsSettings.SecretKey);
            var regionEndpoint = RegionEndpoint.GetBySystemName(awsSettings.Region);

            services.AddSingleton<IAmazonS3>(sp =>
                new AmazonS3Client(credentials, regionEndpoint)
            );

            services.AddScoped<IStorageProvider, AWSImpl>();

            return services;
        }
    }
}