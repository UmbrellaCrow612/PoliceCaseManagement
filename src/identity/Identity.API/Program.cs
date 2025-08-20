using Cache.Redis;
using Identity.API.Extensions;
using Identity.API.Grpc;
using Identity.Application;
using Identity.Infrastructure;
using Logging;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;

SerilogExtensions.ConfigureSerilog();

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddGrpc();
builder.Services.AddOpenApi();
builder.Services.AddBaseAuthorization(config);
builder.Services.AddInfrastructure(config);
builder.Services.AddApplication(config);
builder.Services.AddRedis(config);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    using var scope = app.Services.CreateScope();

    var db = scope.ServiceProvider.GetRequiredService<Identity.Infrastructure.Data.IdentityApplicationDbContext>();
    db.Database.Migrate();
}

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
});

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.MapGrpcService<GRPCUserServiceImpl>();

try
{
    Log.Information("Starting web application");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
