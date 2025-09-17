using Cache.Redis;
using Identity.API.Extensions;
using Identity.API.Grpc;
using Identity.Application;
using Identity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SharedLogging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

LoggingConfigurator.ConfigureLogging(builder);

var config = builder.Configuration;

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

app.UseAuthentication();
app.UseAuthorization();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.MapControllers();

app.MapGrpcService<GRPCUserServiceImpl>();

app.Run();


