using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace SharedLogging
{
    public class LoggingConfigurator
    {
        public static void ConfigureLogging(WebApplicationBuilder builder, string applicationName)
        {
            var environment = builder.Environment.EnvironmentName;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Environment", environment)
                .Enrich.WithProperty("Application", applicationName)
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                .WriteTo.File(
                    path: "Logs/log-.json",
                    rollingInterval: RollingInterval.Day,
                    formatter: new Serilog.Formatting.Json.JsonFormatter())
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = $"{applicationName.ToLower()}-logs-{environment.ToLower()}-{DateTime.UtcNow:yyyy.MM.dd}",
                    MinimumLogEventLevel = LogEventLevel.Debug,
                    DetectElasticsearchVersion = true,
                    InlineFields = true
                })
                .CreateLogger();

            builder.Host.UseSerilog();

            // Optional fallback logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();
        }
    }
}

