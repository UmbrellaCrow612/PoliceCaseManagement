using Identity.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Identity.Core.Models;
using Identity.Application;
using Identity.Application.Settings;
using Scalar.AspNetCore;
using Identity.Infrastructure.Data.Seeding;
using Logging;
using Email;
using Identity.API;

SerilogExtensions.ConfigureSerilog();

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddApplicationCors(config);
builder.Services.AddBaseAuthorization(config);
builder.Services.AddInfrastructure(config);
builder.Services.AddApplicationServices(config);
builder.Services.AddEmailService(config);

var app = builder.Build();

app.UseApplicationCors(builder.Configuration.GetSection("Cors").Get<CORSConfigSettings>() ?? throw new ApplicationException("Cors mising from settings"));

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
});


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    var seeder = new Seeder(userManager, roleManager);

    await seeder.SeeRolesAsync();
    await seeder.SeedUsersAndThereRolesAsync();
}

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
