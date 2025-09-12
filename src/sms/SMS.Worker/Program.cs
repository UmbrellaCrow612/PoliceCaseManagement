using Events.Core;
using Events.Core.Settings;
using MassTransit;
using SMS.Worker.Consumers;

var builder = Host.CreateApplicationBuilder(args);
var config = builder.Configuration;

builder.Services.AddRabbitMqSettings(config);

var rabbitMqSettings = config.GetSection("RabbitMqSettings").Get<RabbitMqSettings>() ?? throw new ApplicationException("Rabbit mq settings missing");

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<SmsRequestEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMqSettings.Host, "/", h =>
        {
            h.Username(rabbitMqSettings.Username);
            h.Password(rabbitMqSettings.Password);
        });

        cfg.ConfigureEndpoints(context);
    });
});

var host = builder.Build();
host.Run();
