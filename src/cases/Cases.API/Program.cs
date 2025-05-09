using Caching;
using Cases.API;
using Cases.Application;
using Cases.Infrastructure;
using Cases.Infrastructure.Data;
using Logging;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;

SerilogExtensions.ConfigureSerilog();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

var config = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddBaseAuthorization(config);
builder.Services.AddCasesInfrastructure(config);
builder.Services.AddCasesApplication(config);
builder.Services.AddCaching(config);
builder.Services.AddValidators();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    // If your reverse proxy is on the same machine or a trusted network,
    // you might not need to specify KnownProxies or KnownNetworks.
    // However, for security, it's good practice if possible.
    // Since Nginx is running as another Docker container on the same Docker network,
    // it's generally considered trusted.
    // If you knew the specific IP of the reverse_proxy container (which can change),
    // you could add it to KnownProxies.
    // For now, clearing them means it will trust any proxy.
    options.KnownProxies.Clear();
    options.KnownNetworks.Clear();
});

var app = builder.Build();

app.UseForwardedHeaders(); // IMPORTANT: Call this early in the pipeline

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
});


app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CasesApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
