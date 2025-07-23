using Cases.Application.Implementations;
using Cases.Core.Services;
using Events.Core;
using Events.Core.Settings;
using Evidence.V1;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StorageProvider.AWS;
using System.Reflection;
using User.V1;

namespace Cases.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddCasesApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAWSStorageProvider(configuration);

            services.AddScoped<ICaseService, CaseService>();
            services.AddScoped<ICaseActionService, CaseActionService>();
            services.AddScoped<ICaseAuthorizationService, CaseAuthorizationService>();
            services.AddScoped<ICaseFileService, CaseFileService>();
            services.AddScoped<IIncidentTypeService, IncidentTypeService>();

            services.AddRabbitMqSettings(configuration);
            var rabbitMqSettings = configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>()
                    ?? throw new ApplicationException("RabbitMqSettings config missing");

            services.AddMassTransit(x =>
            {
                x.AddConsumers(Assembly.GetExecutingAssembly());

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqSettings.Host, "/", h =>
                    {
                        h.Username(rabbitMqSettings.Username);
                        h.Password(rabbitMqSettings.Password);
                    });

                    cfg.ReceiveEndpoint("cases-queue-name", e =>
                    {
                        cfg.ConfigureEndpoints(context);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddGrpcClient<EvidenceService.EvidenceServiceClient>(o =>
            {
                o.Address = new Uri("https://localhost:7078");
            });
            services.AddScoped<EvidenceValidationService>();

            services.AddGrpcClient<UserService.UserServiceClient>(o =>
            {
                o.Address = new Uri("https://localhost:7058");
            });
            services.AddScoped<UserValidationService>();

            services.EnsureDenormalisedEntitiesHaveAConsumer();

            return services;
        }
    }
}
