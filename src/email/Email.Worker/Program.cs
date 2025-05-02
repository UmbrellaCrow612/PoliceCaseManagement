using Email.Worker.Consumers;
using Email.Worker.Settings;
using Events;
using Events.Settings;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);

var config = builder.Configuration;

builder.Services
    .AddOptions<EmailSettings>()
    .Bind(config.GetSection("EmailSettings"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddRabbitMqSettings(config);

var rabbitMqSettings = config.GetSection("RabbitMqSettings").Get<RabbitMqSettings>() ?? throw new ApplicationException("Rabbit MQ settings missing from app settings json");

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<SendEmailConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMqSettings.Host, "/", h =>
        {
            h.Username(rabbitMqSettings.Username);
            h.Password(rabbitMqSettings.Password);
        });

        cfg.ReceiveEndpoint("send-email-queue", e =>
        {
            e.ConfigureConsumer<SendEmailConsumer>(context);
        });
    });
});

var host = builder.Build();
await host.RunAsync();
