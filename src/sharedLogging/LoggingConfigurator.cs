using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace SharedLogging
{
    public class LoggingConfigurator
    {
        public static void ConfigureLogging(WebApplicationBuilder builder)
        {
            // Serilog configuration
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = "myapp-logs-{0:yyyy.MM.dd}", // Daily indices
                    MinimumLogEventLevel = LogEventLevel.Debug,
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

