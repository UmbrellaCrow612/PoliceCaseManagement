using Cases.Application.Consumers;
using Cases.Application.Implementations;
using Cases.Core.Services;
using Events.Settings;
using Events;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cases.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddCasesApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICaseService, CaseService>();


            services.AddRabbitMqSettings(configuration);
            var rabbitMqSettings = configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>()
                    ?? throw new ApplicationException("RabbitMqSettings config missing");

            services.AddMassTransit(x =>
            {
                x.AddConsumer<CaseReportingOfficerValidationConsumer>();
                x.AddConsumer<CaseActionCreatedByValidationEventConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqSettings.Host, "/", h => { 
                        h.Username(rabbitMqSettings.Username); 
                        h.Password(rabbitMqSettings.Password);
                    });

                    cfg.ReceiveEndpoint("cases-queue-name", e => 
                    {
                        e.ConfigureConsumer<CaseReportingOfficerValidationConsumer>(context);
                        e.ConfigureConsumer<CaseActionCreatedByValidationEventConsumer>(context);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
