using Identity.Application;
using Identity.CLI;
using Identity.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

var config = builder.Configuration;

builder.Services.AddInfrastructure(config);
builder.Services.AddConsoleApplicationServices();

builder.Services.AddSingleton<AppRunner>();

using IHost host = builder.Build();

var appRunner = host.Services.GetRequiredService<AppRunner>();

await appRunner.RunAsync();
