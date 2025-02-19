using Email.Service;
using Identity.Infrastructure;
using Identity.Infrastructure.Data.Seeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Serilog;
using Identity.Core.Models;
using Challenge.Core;
using Identity.Infrastructure.Data;
using Authorization.Core;
using Logging.Core;
using Identity.Application.Constants;
using Identity.Application;

SerilogExtensions.ConfigureSerilog();

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                }
            },
            Array.Empty<string>()
        }
    });

    c.AddSecurityDefinition(CustomHeaderOptions.XDeviceFingerprint, new OpenApiSecurityScheme
    {
        Name = CustomHeaderOptions.XDeviceFingerprint,
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "Device fingerprint generated by fingerprintjs"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = CustomHeaderOptions.XDeviceFingerprint
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentCorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Specify your frontend's URL
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Allow cookies and credentials if necessary
    });
});

builder.Services.AddBaseAuthorization(config);
builder.Services.AddChallenges(config);
builder.Services.AddInfrastructure(config);
builder.Services.AddApplicationServices(config);
builder.Services.AddEmailService(config);

var app = builder.Build();

app.UseCors("DevelopmentCorsPolicy");

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
