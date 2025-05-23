using Caching;
using Cases.API;
using Cases.Application;
using Cases.Infrastructure;
using Cases.Infrastructure.Data;
using Logging;
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CasesApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
