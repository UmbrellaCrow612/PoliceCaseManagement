using Cache.Redis;
using Identity.API.Extensions;
using Identity.API.Grpc;
using Identity.Application;
using Identity.Core.Models;
using Identity.Infrastructure;
using Identity.Infrastructure.Data.Seeding;
using Logging;
using Microsoft.AspNetCore.Identity;
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
builder.Services.AddApplicationServices(config);
builder.Services.AddRedis(config);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    using var scope = app.Services.CreateScope();

    var db = scope.ServiceProvider.GetRequiredService<Identity.Infrastructure.Data.IdentityApplicationDbContext>();
    db.Database.Migrate();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    var seeder = new Seeder(userManager, roleManager);

    await seeder.SeeRolesAsync();
    await seeder.SeedUsersAndThereRolesAsync();
}

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
});

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.MapGrpcService<UserServiceImpl>();

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
