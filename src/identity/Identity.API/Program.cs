using Identity.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Identity.Core.Models;
using Identity.Application;
using Scalar.AspNetCore;
using Identity.Infrastructure.Data.Seeding;
using Logging;
using Identity.API.Extensions;
using CORS;
using Caching;
using Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddCaching(config);

var app = builder.Build();

app.UseApplicationCors(config);

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


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<IdentityApplicationDbContext>();
    dbContext.Database.Migrate(); 
}

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
