using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Cases.Core.Services;
using Cases.Infrastructure.Data;
using Cases.Infrastructure.Implementations;
using Cases.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cases.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddCasesInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            }

            services.AddDbContext<CasesApplicationDbContext>(option =>
            {
                option.UseNpgsql(connectionString);
            });

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

            services.AddScoped<IFileUploadService, AWSs3FileUploadService>();

            return services;
        }
    }
}
