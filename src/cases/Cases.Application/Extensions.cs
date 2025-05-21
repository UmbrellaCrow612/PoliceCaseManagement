using Cases.Application.Implementations;
using Cases.Core.Services;
using Events.Settings;
using Events;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using User.V1;

namespace Cases.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddCasesApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICaseService, CaseService>();
            services.AddScoped<ICaseActionService, CaseActionService>();


            services.AddRabbitMqSettings(configuration);
            var rabbitMqSettings = configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>()
                    ?? throw new ApplicationException("RabbitMqSettings config missing");

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqSettings.Host, "/", h => { 
                        h.Username(rabbitMqSettings.Username); 
                        h.Password(rabbitMqSettings.Password);
                    });

                    cfg.ReceiveEndpoint("cases-queue-name", e => 
                    {
                      
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddGrpcClient<UserService.UserServiceClient>(o =>
            {
                o.Address = new Uri("https://localhost:7058");
            });
            services.AddScoped<UserValidationService>();

            return services;
        }
    }
}
