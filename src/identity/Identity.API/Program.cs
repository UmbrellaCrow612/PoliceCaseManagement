using Email.Service;
using Identity.Infrastructure;
using Identity.Infrastructure.Data.Seeding;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Identity.Core.Models;
using Challenge.Core;
using Identity.Infrastructure.Data;
using Authorization.Core;
using Logging.Core;
using Identity.Application;
using Identity.Application.Settings;
using Identity.API.Extensions;

SerilogExtensions.ConfigureSerilog();

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiUI();

builder.Services.AddApplicationCors(config);
builder.Services.AddBaseAuthorization(config);
builder.Services.AddChallenges(config);
builder.Services.AddInfrastructure(config);
builder.Services.AddApplicationServices(config);
builder.Services.AddEmailService(config);

var app = builder.Build();

app.UseApplicationCors(builder.Configuration.GetSection("Cors").Get<CORSConfigSettings>() ?? throw new ApplicationException("Cors mising from settings"));

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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
    var dbContext = scope.ServiceProvider.GetRequiredService<IdentityApplicationDbContext>();

    var rs = new RoleSeeding(roleManager);
    var claimSeeder = new ChallengeClaimSeeding(dbContext);

    await claimSeeder.Seed();

    rs.SeedRoles();
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
