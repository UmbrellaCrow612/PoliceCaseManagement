using Cases.Application.Implementations;
using Cases.Core.Services;
using Events.Core;
using Events.Core.Settings;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StorageProvider.AWS;
using System.Reflection;

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
            services.AddScoped<EvidenceValidationService>();
            services.AddScoped<UserValidationService>();
            services.AddScoped<PersonValidationService>();

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

            services.EnsureDenormalisedEntitiesHaveAConsumer();

            return services;
        }
    }
}
